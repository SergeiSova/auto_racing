using Microsoft.VisualStudio.TestTools.UnitTesting;
using Автогонки;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void DoubleSpeed_DoublesValue()
        {
            Assert.AreEqual(20, GameRules.DoubleSpeed(10));
        }

        [TestMethod]
        public void DoubleSpeed_DoesNotExceedMaximum()
        {
            Assert.AreEqual(GameRules.MaxSpeed, GameRules.DoubleSpeed(GameRules.MaxSpeed));
        }

        [DataTestMethod]
        [DataRow("10", 10.0)]
        [DataRow("10,5", 10.5)]
        [DataRow("10.5", 10.5)]
        public void TryParseSpeed_AcceptsPositiveNumbers(string text, double expected)
        {
            Assert.IsTrue(GameRules.TryParseSpeed(text, out double speed));
            Assert.AreEqual(expected, speed);
        }

        [DataTestMethod]
        [DataRow("-10")]
        [DataRow("0")]
        [DataRow("Скорость")]
        [DataRow(".,!?")]
        [DataRow("1000")]
        public void TryParseSpeed_RejectsInvalidValues(string text)
        {
            Assert.IsFalse(GameRules.TryParseSpeed(text, out _));
        }
    }
}
