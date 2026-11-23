using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;

namespace Автогонки
{
    public class PlayerStoreException : Exception
    {
        public PlayerStoreException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }

    [DataContract]
    public class PlayerRecord
    {
        [DataMember] public string Login { get; set; }
        [DataMember] public string Salt { get; set; }
        [DataMember] public string Hash { get; set; }
        [DataMember] public int Iterations { get; set; }
    }

    [DataContract]
    public class GameResult
    {
        [DataMember] public string Login { get; set; }
        [DataMember] public string PlayedAt { get; set; }
        [DataMember] public double Seconds { get; set; }
        [DataMember] public int Coins { get; set; }
    }

    [DataContract]
    internal class PlayerStoreData
    {
        [DataMember] public List<PlayerRecord> Players { get; set; }
        [DataMember] public List<GameResult> Results { get; set; }
    }

    /// <summary>Локальное хранилище игроков и результатов.</summary>
    public class PlayerStore
    {
        public const int MinLoginLength = 3;
        public const int MaxLoginLength = 20;
        public const int MinPasswordLength = 3;
        private const int MaxResults = 1000;
        private const int Iterations = 100000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        private readonly string filePath;

        public PlayerStore() : this(Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Автогонки", "players.json"))
        {
        }

        public PlayerStore(string filePath)
        {
            this.filePath = filePath;
        }

        public bool Register(string login, string password, out string error)
        {
            login = (login ?? string.Empty).Trim();
            password = password ?? string.Empty;

            if (login.Length < MinLoginLength || login.Length > MaxLoginLength)
            {
                error = $"Логин должен содержать от {MinLoginLength} до {MaxLoginLength} символов";
                return false;
            }
            if (password.Length < MinPasswordLength)
            {
                error = $"Пароль должен содержать не меньше {MinPasswordLength} символов";
                return false;
            }

            PlayerStoreData data = Load();
            if (FindPlayer(data, login) != null)
            {
                error = "Игрок с таким логином уже зарегистрирован";
                return false;
            }

            byte[] salt = new byte[SaltSize];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(salt);
            }

            data.Players.Add(new PlayerRecord
            {
                Login = login,
                Salt = Convert.ToBase64String(salt),
                Hash = Convert.ToBase64String(DeriveHash(password, salt, Iterations)),
                Iterations = Iterations
            });
            Save(data);
            error = null;
            return true;
        }

        public bool Authenticate(string login, string password)
        {
            PlayerRecord player = FindPlayer(Load(), (login ?? string.Empty).Trim());
            if (player == null || password == null)
            {
                return false;
            }

            try
            {
                byte[] salt = Convert.FromBase64String(player.Salt);
                byte[] expected = Convert.FromBase64String(player.Hash);
                return FixedTimeEquals(DeriveHash(password, salt, player.Iterations), expected);
            }
            catch (Exception e) when (e is FormatException || e is ArgumentException)
            {
                throw new PlayerStoreException("Повреждена запись игрока", e);
            }
        }

        public void SaveResult(string login, double seconds, int coins)
        {
            PlayerStoreData data = Load();
            data.Results.Add(new GameResult
            {
                Login = login,
                PlayedAt = DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                Seconds = Math.Round(seconds, 1),
                Coins = coins
            });
            if (data.Results.Count > MaxResults)
            {
                data.Results.RemoveRange(0, data.Results.Count - MaxResults);
            }
            Save(data);
        }

        public IList<GameResult> GetResults()
        {
            return Enumerable.Reverse(Load().Results).ToList();
        }

        private static PlayerRecord FindPlayer(PlayerStoreData data, string login)
        {
            return data.Players.FirstOrDefault(p => string.Equals(p.Login, login, StringComparison.OrdinalIgnoreCase));
        }

        private static byte[] DeriveHash(string password, byte[] salt, int iterations)
        {
            using (Rfc2898DeriveBytes derive = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                return derive.GetBytes(HashSize);
            }
        }

        private static bool FixedTimeEquals(byte[] left, byte[] right)
        {
            if (left.Length != right.Length)
            {
                return false;
            }
            int difference = 0;
            for (int i = 0; i < left.Length; i++)
            {
                difference |= left[i] ^ right[i];
            }
            return difference == 0;
        }

        private PlayerStoreData Load()
        {
            try
            {
                if (!File.Exists(filePath))
                {
                    return new PlayerStoreData { Players = new List<PlayerRecord>(), Results = new List<GameResult>() };
                }

                using (FileStream stream = File.OpenRead(filePath))
                {
                    PlayerStoreData data = (PlayerStoreData)new DataContractJsonSerializer(typeof(PlayerStoreData)).ReadObject(stream);
                    if (data == null)
                    {
                        throw new InvalidDataException("Файл данных пуст");
                    }
                    data.Players = data.Players ?? new List<PlayerRecord>();
                    data.Results = data.Results ?? new List<GameResult>();
                    return data;
                }
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException || e is SerializationException)
            {
                throw new PlayerStoreException("Не удалось прочитать данные игроков", e);
            }
        }

        private void Save(PlayerStoreData data)
        {
            string tempPath = filePath + ".tmp";
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(filePath));
                using (FileStream stream = File.Create(tempPath))
                {
                    new DataContractJsonSerializer(typeof(PlayerStoreData)).WriteObject(stream, data);
                }

                if (File.Exists(filePath))
                {
                    File.Replace(tempPath, filePath, null);
                }
                else
                {
                    File.Move(tempPath, filePath);
                }
            }
            catch (Exception e) when (e is IOException || e is UnauthorizedAccessException)
            {
                throw new PlayerStoreException("Не удалось сохранить данные игроков", e);
            }
        }
    }
}
