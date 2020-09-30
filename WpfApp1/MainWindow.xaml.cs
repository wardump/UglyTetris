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
                {null, NewRectangle(Colors.Red), NewRectangle(Colors.Red), null},
                {null, null,  NewRectangle(Colors.Red), null},
                {null, null,  NewRectangle(Colors.Red), null},
                {null, null,  null, null}
            };

            TestFigure.Draw(fx*FieldHelper.BlockWidth, fy*FieldHelper.BlockHeight);

            Field = CreateField(FieldHelper.FieldDefaultWidth, FieldHelper.FieldDefaultHeight);
            RedrawField();

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

        public Rectangle[,] Field;

        private Rectangle[,] CreateField(int width, int height)
        {
            var field = new Rectangle[width+2, height+1];

            for (var i = 0; i < height+1; i++) // +1 each side for the walls
            {
                field[0,i] = NewRectangle(Colors.DimGray);
                field[width+1, i] = NewRectangle(Colors.DimGray);
            }

            for (var i = 1; i < width + 1; i++)
            {
                field[i, height] = NewRectangle(Colors.DimGray);
            }

            return field;
        }

        private void RedrawField()
        {
            var rectangles = new HashSet<Rectangle>();
            
            for (var i = 0; i < TestFigure.Width; i++)
            for (var j = 0; j < TestFigure.Height; j++)
            {
                if (TestFigure.Tiles[i, j] != null)
                    rectangles.Add(TestFigure.Tiles[i, j]);
            }
            
            for (var i = Field.GetLowerBound(0); i <= Field.GetUpperBound(0); i++)
            {
                for (var j = Field.GetLowerBound(1); j <= Field.GetUpperBound(1) ; j++)
                {
                    if (Field[i,j] != null)
                    {
                        rectangles.Add(Field[i, j]);
                        Canvas.SetLeft(Field[i, j], i * FieldHelper.BlockWidth + 1);
                        Canvas.SetTop(Field[i, j], j * FieldHelper.BlockHeight + 1);
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

            TestFigure.Draw(fx* FieldHelper.BlockWidth, fy* FieldHelper.BlockHeight);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            fx++;

            if (!Check(fx, fy))
            {
                fx--;
                return;
            }

            TestFigure.Draw(fx* FieldHelper.BlockWidth, fy* FieldHelper.BlockHeight);
        }

        public bool Check(int x, int y)
        {
            for (var i = x; i < x + TestFigure.Width; i++)
            {
                for (var j = y; j < y + TestFigure.Height; j++)
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
        
        private Rectangle NewRectangle(Color color)
        {
            var r = new Rectangle
            {
                Width = FieldHelper.BlockWidth * 0.9,
                Height = FieldHelper.BlockHeight * 0.9,
                Fill = new SolidColorBrush(color)
            };
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
            TestFigure.Draw(fx* FieldHelper.BlockWidth, fy* FieldHelper.BlockHeight);
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            TestFigure.RotateRight();
            if (!Check(fx, fy))
            {
                TestFigure.RotateLeft();
            }
            TestFigure.Draw(fx* FieldHelper.BlockWidth, fy* FieldHelper.BlockHeight);
        }

        public static MainWindow Instance;

        public void SetRandomFigure()
        {
            TestFigure = new Figure();

            var r = new Random();
            switch (r.Next(1, 5))
            {
                case 1:
                    TestFigure.Tiles = new Rectangle[,]
                    {
                        {null, NewRectangle(Colors.Red), NewRectangle(Colors.Red), null},
                        {null, null, NewRectangle(Colors.Red), null},
                        {null, null, NewRectangle(Colors.Red), null},
                        {null, null, null, null}
                    };
                    break;
                case 2:
                    TestFigure.Tiles = new Rectangle[,]
                    {
                        {null, NewRectangle(Colors.LawnGreen), NewRectangle(Colors.LawnGreen), null},
                        {null, NewRectangle(Colors.LawnGreen), null, null},
                        {null, NewRectangle(Colors.LawnGreen), null, null},
                        {null, null, null, null}
                    };
                    break;
                case 3:
                    TestFigure.Tiles = new Rectangle[,]
                    {
                        {null, null, null, null},
                        {null, NewRectangle(Colors.Brown), NewRectangle(Colors.Brown), null},
                        {null, NewRectangle(Colors.Brown), NewRectangle(Colors.Brown), null},
                        {null, null, null, null}
                    };
                    break;
                case 4:
                    TestFigure.Tiles = new Rectangle[,]
                    {
                        {null, NewRectangle(Colors.DeepSkyBlue), null, null},
                        {null, NewRectangle(Colors.DeepSkyBlue), null, null},
                        {null, NewRectangle(Colors.DeepSkyBlue), null, null},
                        {null, NewRectangle(Colors.DeepSkyBlue), null, null}
                    };
                    break;

                //todo add more figures
            }


            RedrawField();
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
