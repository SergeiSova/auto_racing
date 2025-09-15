using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfApp7;

namespace Автогонки
{
    public partial class MainWindow : Window
    {
        DispatcherTimer gameTimer = new DispatcherTimer();
        List<Rectangle> itemRemover = new List<Rectangle>();

        Random rand = new Random();

        ImageBrush playerImage = new ImageBrush();
        ImageBrush coinImage = new ImageBrush();

        Rect playerHitBox;

        public double speed = 15;
        int playerSpeed = 10;
        int carNum;
        int coinCounter = 10;
        int coin = 0;
        public double login;
        string password;
 
        double score;

        bool moveLeft, moveRight, gameOver;
        public MainWindow()
        {
            InitializeComponent();


            myCanvas.Focus();

            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);

            Challenge.Content = "Сложность: средняя";
            gameOver = true;

            Login Login = new Login();

            if (Login.ShowDialog() == true) ;
                //login = string.Parse(Login.login);
        }

        private void GameLoop(object sender, EventArgs e)
        {
            score += .05;

            coinCounter -= 1;

            scoreText.Content = "Время: " + score.ToString("#.#") + " секунд";
            coinText.Content = "Монет: " + coin;
            playerHitBox = new Rect(Canvas.GetLeft(player), Canvas.GetTop(player), player.Width, player.Height);

            if (moveLeft == true && Canvas.GetLeft(player) > 0)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) - playerSpeed);
            }
            if (moveRight == true && Canvas.GetLeft(player) + 90 < Application.Current.MainWindow.Width)
            {
                Canvas.SetLeft(player, Canvas.GetLeft(player) + playerSpeed);
            }

            if (coinCounter < 1)
            {
                MakeCoin();
                coinCounter = rand.Next(100, 150);
            }

            foreach (var x in myCanvas.Children.OfType<Rectangle>())
            {

                if ((string)x.Tag == "roadMarks")
                {

                    Canvas.SetTop(x, Canvas.GetTop(x) + speed); 

                    if (Canvas.GetTop(x) > 510)
                    {
                        Canvas.SetTop(x, -152);
                    }

                } 

                if ((string)x.Tag == "Car")
                {

                    Canvas.SetTop(x, Canvas.GetTop(x) + speed);

                    if (Canvas.GetTop(x) > 500)
                    {
                        ChangeCars(x);
                    }

                    Rect carHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (playerHitBox.IntersectsWith(carHitBox))
                    {

                        gameTimer.Stop(); 
                        gameOver = true; 
                        MessageBox.Show("Нажмите Enter для повторного запуска игры", "Конец игры");
                    }
                }

                if ((string)x.Tag == "coin")
                {
                    
                    Canvas.SetTop(x, Canvas.GetTop(x) + 5);

                    Rect coinHitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                    if (playerHitBox.IntersectsWith(coinHitBox))
                    {
                        itemRemover.Add(x);
                        coin++;

                    }

                    if (Canvas.GetTop(x) > 400)
                    {
                        itemRemover.Add(x);
                    }

                }
            } 

            foreach (Rectangle y in itemRemover)
            {
                myCanvas.Children.Remove(y);
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            
            if (e.Key == Key.Left)
            {
                moveLeft = true;
            }
            if (e.Key == Key.Right)
            {
                moveRight = true;
            }
        }

        private void OnKeyUP(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                moveLeft = false;
            }
            if (e.Key == Key.Right)
            {
                moveRight = false;
            }
          
            if (e.Key == Key.Enter && gameOver == true)
            {
                
                StartGame();
            }
        }

        private void StartGame()
        {
            gameTimer.Start();
            moveLeft = false;
            moveRight = false;
            gameOver = false;
            coin = 0;
            score = 0;
            
            scoreText.Content = "Время: 0 секунд";
            coinText.Content = "Очков: 0";

            playerImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/игрок.png"));
            coinImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/монета.png"));
            
            player.Fill = playerImage;
            
            myCanvas.Background = Brushes.Gray;

            foreach (var x in myCanvas.Children.OfType<Rectangle>())
            {
               
                if ((string)x.Tag == "Car")
                {
                    Canvas.SetTop(x, (rand.Next(100, 400) * -1));
                    Canvas.SetLeft(x, rand.Next(0, 430));
                   
                    ChangeCars(x);
                }

                if ((string)x.Tag == "coin")
                {
                    itemRemover.Add(x);
                }

            }
            itemRemover.Clear();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void Start_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }
        
        private void easy_Click(object sender, RoutedEventArgs e)
        {
            speed = 10;
            Challenge.Content = "Сложность: низкая" ;
        }

        private void normal_Click(object sender, RoutedEventArgs e)
        {
            speed = 15;
            Challenge.Content = "Сложность: средняя"; 
        }

        private void hard_Click(object sender, RoutedEventArgs e)
        {
            speed = 20;
            Challenge.Content = "Сложность: высокая";
        }

        private void x2_Click(object sender, RoutedEventArgs e)
        {
            speed = speed * 2;
            Challenge.Content = "Сложность: "+ speed;
        }

        

        private void my_Click(object sender, RoutedEventArgs e)
        {
            myspeed myspeed = new myspeed();

           if (myspeed.ShowDialog() == true)

                // try
                // {
                speed = double.Parse(myspeed.mspee);
                
            Challenge.Content = "Сложность: своя (" + speed +")";
          // }
           
           /* catch (System.FormatException)
           {
                    MessageBox.Show("Некорректный ввод", "Ошибка");
           }

            if (speed <= 0)
            {
                speed = 15;
                Challenge.Content = "Сложность: средняя";
                MessageBox.Show("Некорректный ввод", "Ошибка");
                
            } */

        }

        /*private void no_Click(object sender, RoutedEventArgs e)
        {
            Hard.Content = "Hard is no" + speed;
            speed = 8;
            if (score >= 10 && score < 20)
            {
                speed = 12;
            }

            if (score >= 20 && score < 30)
            {
                speed = 14;
            }
            if (score >= 30 && score < 40)
            {
                speed = 16;
            }
            if (score >= 40 && score < 50)
            {
                speed = 18;
            }
            if (score >= 50 && score < 80)
            {
                speed = 22;
            }

        } */

        private void ChangeCars(Rectangle car)
        {

            carNum = rand.Next(1, 6); 

            ImageBrush carImage = new ImageBrush(); 

            switch (carNum)
            {
                case 1:
                    carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/машина1.png"));
                    break;
                case 2:
                    carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/машина2.png"));
                    break;
                case 3:
                    carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/машина3.png"));
                    break;
                case 4:
                    carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/машина4.png"));
                    break;
                case 5:
                    carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/машина5.png"));
                    break;
                case 6:
                    carImage.ImageSource = new BitmapImage(new Uri("pack://application:,,,/images/машина6.png"));
                    break;
            }

            car.Fill = carImage;

            Canvas.SetTop(car, (rand.Next(100, 400) * -1));
            Canvas.SetLeft(car, rand.Next(0, 430));
        }

        private void MakeCoin()
        {
            Rectangle newCoin = new Rectangle
            {
                Height = 50,
                Width = 50,
                Tag = "coin",
                Fill = coinImage
            };

            Canvas.SetLeft(newCoin, rand.Next(0, 430));
            Canvas.SetTop(newCoin, (rand.Next(100, 400) * -1));

            myCanvas.Children.Add(newCoin);

        }

        
    }
}