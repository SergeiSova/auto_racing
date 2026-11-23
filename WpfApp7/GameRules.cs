using System;
using System.Globalization;

namespace Автогонки
{
    public static class GameRules
    {
        public const double MaxSpeed = 60;

        public static double DoubleSpeed(double speed)
        {
            return Math.Min(speed * 2, MaxSpeed);
        }

        public static bool TryParseSpeed(string text, out double speed)
        {
            string normalized = (text ?? string.Empty).Trim().Replace(',', '.');
            return double.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out speed)
                && speed > 0
                && speed <= MaxSpeed;
        }
    }
}
