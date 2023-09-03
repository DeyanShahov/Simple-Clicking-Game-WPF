using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Simple_Clicking_Game_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random random = new Random();
        DispatcherTimer gameTimer = new DispatcherTimer();
        List<Shape> removeThis = new List<Shape>();

        int spawnRate = 60;
        int currentRate;
        int lastScore = 0;
        int health = 350;
        int posX;
        int posY;
        int score = 0;

        double growthRate = 0.5;

        MediaPlayer playClickSound = new MediaPlayer();
        MediaPlayer playerPopSound = new MediaPlayer();

        Uri ClickedSound;
        Uri PoppedSound;

        Brush brush;

        public MainWindow()
        {
            InitializeComponent();

            gameTimer.Tick += GameLoop;
            gameTimer.Interval = TimeSpan.FromMilliseconds(20);
            gameTimer.Start();

            currentRate = spawnRate;

            ClickedSound = new Uri("pack://siteoforigin:,,,/Sound/clickedpop.mp3");
            PoppedSound = new Uri("pack://siteoforigin:,,,/Sound/pop.mp3");
        }

        private void GameLoop(object? sender, EventArgs e)
        {
            txtScore.Content = "Score: " + score;
            txtLastScore.Content = "Last Score: " + lastScore;

            currentRate -= 2;

            if (currentRate < 1) CreateNewFigure();


            foreach (var shape in MyCanvas.Children.OfType<Shape>())
            {
                if (shape.Name != "healthBar")
                {
                    double newX = Canvas.GetLeft(shape) - growthRate / 2;
                    double newY = Canvas.GetTop(shape) - growthRate / 2;

                    shape.Height += growthRate;
                    shape.Width += growthRate;

                    Canvas.SetLeft(shape, newX);
                    Canvas.SetTop(shape, newY);

                    CheckForAutoDestroy(shape);
                }
            }

            if (health > 1) healthBar.Width = health;
            else GameOverFunction();

            foreach (Shape el in removeThis)
            {
                MyCanvas.Children.Remove(el);
            }


            if (score > 5) spawnRate = 25;

            if (score > 10)
            {
                spawnRate = 15;
                growthRate = 1.5;
            }
        }

        private void CheckForAutoDestroy(Shape shape)
        {
            if (shape.Width > 70)
            {
                removeThis.Add(shape);

                if (shape.Tag.ToString() == "circle") health -= 15;
                else if (shape.Tag.ToString() == "cub") health += (health < 350) ? 5 : 0;

                playerPopSound.Open(PoppedSound);
                playerPopSound.Play();
            }
        }

        private void CreateNewFigure()
        {
            currentRate = spawnRate;

            posX = random.Next(15, 700);
            posY = random.Next(50, 350);

            brush = new SolidColorBrush(Color.FromRgb((byte)random.Next(1, 255), (byte)random.Next(1, 255), (byte)random.Next(1, 255)));

            int numberFigure = random.Next(0, 3);

            Shape newShape;

            if (numberFigure == 0 || numberFigure == 1) newShape = ShapeFactory.CreateCircle(brush);
            else newShape = ShapeFactory.CreateRectangle(brush);

            Canvas.SetLeft(newShape, posX);
            Canvas.SetTop(newShape, posY);

            MyCanvas.Children.Add(newShape);
        }

        private void GameOverFunction()
        {
            gameTimer.Stop();
            MessageBox.Show("Game Over" + Environment.NewLine + "You Scored: " + score + Environment.NewLine + "Click Ok to play again!");

            foreach (var item in MyCanvas.Children.OfType<Shape>())
            {
                if (item.Name != "healthBar") removeThis.Add(item);
            }

            foreach (Shape el in removeThis)
            {
                MyCanvas.Children.Remove(el);
            }


            growthRate = 0.6;
            spawnRate = 60;
            lastScore = score;
            score = 0;
            currentRate = 5;
            health = 350;
            removeThis.Clear();
            gameTimer.Start();
        }

        private void MyCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is FrameworkElement element && (e.OriginalSource is Ellipse || e.OriginalSource is Rectangle))
            {
                MyCanvas.Children.Remove(element);

                if (element is Ellipse) score++;
                else if (element is Rectangle) score--;

                playClickSound.Open(ClickedSound);
                playClickSound.Play();
            }
        }

    }
}
