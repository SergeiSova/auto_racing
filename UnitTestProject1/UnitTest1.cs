using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Автогонки;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod()
        {
            double speed = 10;
            double x2speed = 2;
            // myspeed m = new myspeed();
            double excpected = 20;
            MainWindow W = new MainWindow();
            double actual = W.test_m(speed, x2speed);
            Assert.AreEqual(excpected, actual, "не корректные данные");
        }
    }
}
