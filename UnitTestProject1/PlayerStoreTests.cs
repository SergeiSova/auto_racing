using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Автогонки;

namespace UnitTestProject1
{
    [TestClass]
    public class PlayerStoreTests
    {
        private string directory;
        private string filePath;

        [TestInitialize]
        public void SetUp()
        {
            directory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            filePath = Path.Combine(directory, "players.json");
        }

        [TestCleanup]
        public void TearDown()
        {
            if (Directory.Exists(directory))
            {
                Directory.Delete(directory, true);
            }
        }

        [TestMethod]
        public void RegisteredPlayerCanSignIn()
        {
            Assert.IsTrue(new PlayerStore(filePath).Register("user", "secret", out _));
            Assert.IsTrue(new PlayerStore(filePath).Authenticate("USER", "secret"));
            Assert.IsFalse(new PlayerStore(filePath).Authenticate("user", "wrong"));
        }

        [TestMethod]
        public void DuplicateLoginIsRejected()
        {
            PlayerStore store = new PlayerStore(filePath);
            Assert.IsTrue(store.Register("user", "secret", out _));
            Assert.IsFalse(store.Register("USER", "other", out string error));
            Assert.IsNotNull(error);
        }

        [TestMethod]
        public void PasswordIsNotStoredAsPlainText()
        {
            new PlayerStore(filePath).Register("user", "secret-password", out _);
            StringAssert.DoesNotMatch(File.ReadAllText(filePath), new System.Text.RegularExpressions.Regex("secret-password"));
        }

        [TestMethod]
        public void ResultsAreSavedNewestFirst()
        {
            PlayerStore store = new PlayerStore(filePath);
            store.SaveResult("user", 10.04, 2);
            store.SaveResult("user", 20, 5);
            var results = new PlayerStore(filePath).GetResults();
            Assert.AreEqual(2, results.Count);
            Assert.AreEqual(5, results[0].Coins);
            Assert.AreEqual(10.0, results[1].Seconds);
        }
    }
}
