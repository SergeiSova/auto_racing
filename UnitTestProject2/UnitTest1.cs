using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Автогонки;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.Factory;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowStripControls;
using TestStack.White.InputDevices;

namespace UnitTestProject2
{
    [TestClass]
    public class UnitTest1
    {
       
        [TestMethod]
        public void TestDoSomething1()
        {
            var app = Application.Launch("WpfApp7.exe");

            var window = app.GetWindow("MainWindow", InitializeOption.NoCache);

            var MyButton = window.Get<Button>("Start");
                MyButton.Click();

            var Dialog = window.ModalWindow("Конец игры");

            var MyButton1 = Dialog.Get<Button>("ОК");
                MyButton1.Click();

            var Button3 = window.Get<Button>("Exit");
                Button3.Click();

            app.Close();  
                
        }

        
        [TestMethod]
        public void TestDoSomething2()
        {
            var app = Application.Launch("WpfApp7.exe");

            var window = app.GetWindow("MainWindow", InitializeOption.NoCache);

            var MyButton = window.Get<Button>("Start");
          
            var MyButton2 = window.MenuBar.MenuItem("Сложность", "низкая");
            MyButton2.Click();
            MyButton.Click();

            var Dialog1 = window.ModalWindow("Конец игры");
            var Button11 = Dialog1.Get<Button>("ОК");
            Button11.Click();

            var MyButton21 = window.MenuBar.MenuItem("Сложность", "средняя");
            MyButton21.Click();
            MyButton.Click();
            
            var Dialog11 = window.ModalWindow("Конец игры");
            var Button111 = Dialog11.Get<Button>("ОК");
            Button111.Click();

            var MyButton22 = window.MenuBar.MenuItem("Сложность", "высокая");
            MyButton22.Click();
            MyButton.Click();

            var Dialog111 = window.ModalWindow("Конец игры");
            var Button1111 = Dialog111.Get<Button>("ОК");
            Button1111.Click();

            var Button3 = window.Get<Button>("Exit");
            Button3.Click();

            app.Close();

        }

        [TestMethod]
        public void TestDoSomething3()
        {
            var app = Application.Launch("WpfApp7.exe");

            var window = app.GetWindow("MainWindow", InitializeOption.NoCache);

            var MyButton = window.Get<Button>("Start");
            
            MyButton.Click();

            var Dialog1 = window.ModalWindow("Конец игры");
            var Button11 = Dialog1.Get<Button>("ОК");
            Button11.Click();

            var MyButton21 = window.MenuBar.MenuItem("Сложность", "x2");
            MyButton21.Click();
            MyButton.Click();

            var Dialog11 = window.ModalWindow("Конец игры");
            var Button111 = Dialog11.Get<Button>("ОК");
            Button111.Click();

            var Button3 = window.Get<Button>("Exit");
            Button3.Click();

            app.Close();

        }
    }
}