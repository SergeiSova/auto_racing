using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Автогонки;

namespace WpfApp7
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public Login()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Singup_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        public string log
        {

            get { return login.Text; }
        }

        public string pass
        {

            get { return password.Text; }
        }
    }
}
