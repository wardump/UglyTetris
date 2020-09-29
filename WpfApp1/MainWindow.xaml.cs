using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Instance = this;
            InitializeComponent();

            TestFigure.Tiles = new Rectangle[,]
            {
                {null, NewRectangle(), NewRectangle(), null},
                {null, null,  NewRectangle(), null},
                {null, null,  NewRectangle(), null},
                {null, null,  null, null}
            };

            TestFigure.Draw(fx*20, fy*20);

            DrawField();

            timer = new System.Windows.Threading.DispatcherTimer {Interval = TimeSpan.FromMilliseconds(10)};

            timer.Tick += (sender, args) =>
            {
                game.Tick();
            };

            timer.Start();
        }

        public Figure TestFigure = new Figure();

        public int fx = 6;
        public int fy = 0;

        public Game game = new Game();
        public System.Windows.Threading.DispatcherTimer timer;

        public Rectangle[,] Field = new Rectangle[14,25];

        private void DrawField()
        {
            for (var i = 0; i < 25; i++)
            {
                Field[0,i] = NewColorRectangle(Colors.DimGray);
                Field[13, i] = NewColorRectangle(Colors.DimGray);
            }

            for (var i = 1; i < 13; i++)
            {
                Field[i, 24] = NewColorRectangle(Colors.DimGray);
            }

            DrawField2();
        }

        private void DrawField2()
        {
            var rectangles = new HashSet<Rectangle>();
            
            for (var i = 0; i < 4; i++)
            for (var j = 0; j < 4; j++)
            {
                if (TestFigure.Tiles[i, j] != null)
                    rectangles.Add(TestFigure.Tiles[i, j]);
            }
            
            for (var i = 0; i < 14; i++)
            {
                for (var j = 0; j < 25; j++)
                {
                    if (Field[i,j] != null)
                    {
                        rectangles.Add(Field[i, j]);
                        Canvas.SetLeft(Field[i, j], i * 20 + 1);
                        Canvas.SetTop(Field[i, j], j * 20 + 1);
                    }
                }
            }

            foreach (var rect in MainCanvas.Children.OfType<Rectangle>().ToList())
            {
                if (!rectangles.Contains(rect))
                {
                    MainCanvas.Children.Remove(rect);
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            fx--;

            if (!Check(fx, fy))
            {
                fx++;
                return;
            }

            TestFigure.Draw(fx*20, fy*20);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            fx++;

            if (!Check(fx, fy))
            {
                fx--;
                return;
            }

            TestFigure.Draw(fx*20, fy*20);
        }

        public bool Check(int x, int y)
        {
            for (var i = x; i < x + 4; i++)
            {
                for (var j = y; j < y + 4; j++)
                {
                    var r = TestFigure.Tiles[i - x, j - y];
                    if (r == null)
                    {
                        continue;
                    }

                    if (Field[i, j] != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private Rectangle NewRectangle()
        {
            var r = new Rectangle();
            r.Width = 18;
            r.Height = 18;
            r.Fill = new SolidColorBrush(Colors.Red);
            MainCanvas.Children.Add(r);
            return r;
        }

        private Rectangle NewColorRectangle(Color color)
        {
            var r = new Rectangle();
            r.Width = 18;
            r.Height = 18;
            r.Fill = new SolidColorBrush(color);
            MainCanvas.Children.Add(r);
            return r;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TestFigure.RotateLeft();
            if (!Check(fx, fy))
            {
                TestFigure.RotateRight();
            }
            TestFigure.Draw(fx*20, fy*20);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TestFigure.RotateRight();
            if (!Check(fx, fy))
            {
                TestFigure.RotateLeft();
            }
            TestFigure.Draw(fx*20, fy*20);
        }

        public static MainWindow Instance;

        public void SetRandomFigure()
        {
            TestFigure = new Figure();

            var r = new Random();
            switch (r.Next(1, 4))
            {
                case 1:
                    TestFigure.Tiles = new Rectangle[,]
                    {
                        {null, NewRectangle(), NewRectangle(), null},
                        {null, null, NewRectangle(), null},
                        {null, null, NewRectangle(), null},
                        {null, null, null, null}
                    };
                    break;
                case 2:
                    TestFigure.Tiles = new Rectangle[,]
                    {
                        {null, NewColorRectangle(Colors.LawnGreen), NewColorRectangle(Colors.LawnGreen), null},
                        {null, NewColorRectangle(Colors.LawnGreen), null, null},
                        {null, NewColorRectangle(Colors.LawnGreen), null, null},
                        {null, null, null, null}
                    };
                    break;
                case 3:
                    TestFigure.Tiles = new Rectangle[,]
                    {
                        {null, null, null, null},
                        {null, NewColorRectangle(Colors.Brown), NewColorRectangle(Colors.Brown), null},
                        {null, NewColorRectangle(Colors.Brown), NewColorRectangle(Colors.Brown), null},
                        {null, null, null, null}
                    };
                    break;
                case 4:
                    TestFigure.Tiles = new Rectangle[,]
                    {
                        {null, NewColorRectangle(Colors.DeepSkyBlue), null, null},
                        {null, NewColorRectangle(Colors.DeepSkyBlue), null, null},
                        {null, NewColorRectangle(Colors.DeepSkyBlue), null, null},
                        {null, NewColorRectangle(Colors.DeepSkyBlue), null, null}
                    };
                    break;

                //todo add more figures
            }


            DrawField2();
            LineCountTextBlock.Text = game.Lines.ToString(CultureInfo.InvariantCulture);

            if (!FieldHelper.CheckFigure(Field, TestFigure, fx, fy))
            {
                timer.Stop();
                MessageBox.Show("GAME OVER");
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.IsRepeat)
            {
                return;
            }

            if (e.Key == Key.Left)
            {
                Button_Click(sender, null);
            }
            else if (e.Key == Key.Right)
            {
                Button_Click_3(sender, null);
            }
            else if (e.Key == Key.Up)
            {
                Button_Click_1(sender, null);
            }
            else if (e.Key == Key.Down)
            {
                Button_Click_2(sender, null);
            }

            else if (e.Key == Key.Space)
            {
                game.IsFalling = true;
            }
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            game.IsFalling = true;
        }
    }
}
