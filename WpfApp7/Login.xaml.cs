using System.Windows;
using Автогонки;

namespace WpfApp7
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        private readonly PlayerStore store;

        public Login(PlayerStore store)
        {
            this.store = store;
            InitializeComponent();
            login.Focus();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            Submit(false);
        }

        private void Singup_Click(object sender, RoutedEventArgs e)
        {
            Submit(true);
        }

        private void Submit(bool register)
        {
            bool emptyLogin = string.IsNullOrWhiteSpace(login.Text);
            bool emptyPassword = string.IsNullOrEmpty(password.Password);

            if (emptyLogin && emptyPassword)
            {
                MessageBox.Show("Поля «Логин» и «Пароль» не должны быть пустыми");
                return;
            }
            if (emptyLogin)
            {
                MessageBox.Show("Поле «Логин» не должно быть пустым");
                return;
            }
            if (emptyPassword)
            {
                MessageBox.Show("Поле «Пароль» не должно быть пустым");
                return;
            }

            try
            {
                if (register)
                {
                    if (!store.Register(login.Text, password.Password, out string error))
                    {
                        MessageBox.Show(error);
                        return;
                    }
                    MessageBox.Show("Вы зарегистрировались");
                }
                else if (!store.Authenticate(login.Text, password.Password))
                {
                    MessageBox.Show("Неверный логин или пароль");
                    return;
                }
                DialogResult = true;
            }
            catch (PlayerStoreException)
            {
                MessageBox.Show("Ошибка обращения к базе данных!");
            }
        }

        public string log
        {
            get { return login.Text.Trim(); }
        }
    }
}
