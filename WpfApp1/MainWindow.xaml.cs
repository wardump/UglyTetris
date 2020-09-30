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

            Figure.Tiles = new Rectangle[,]
            {
                {null, NewRectangle(Colors.Red), NewRectangle(Colors.Red), null},
                {null, null, NewRectangle(Colors.Red), null},
                {null, null, NewRectangle(Colors.Red), null},
                {null, null, null, null}
            };

            Figure.Draw(FigurePositionX * FieldHelper.BlockWidth, FigurePositionY * FieldHelper.BlockHeight);

            Field = CreateField(FieldHelper.FieldDefaultWidth, FieldHelper.FieldDefaultHeight);
            RedrawField();

            _timer = new System.Windows.Threading.DispatcherTimer {Interval = TimeSpan.FromMilliseconds(10)};

            _timer.Tick += (sender, args) => { Game.Tick(); };

            _timer.Start();
        }

        public Figure Figure = new Figure();

        public int FigurePositionX = 6;
        public int FigurePositionY = 0;

        public Game Game = new Game();
        private System.Windows.Threading.DispatcherTimer _timer;

        public Rectangle[,] Field;

        private Rectangle[,] CreateField(int width, int height)
        {
            var field = new Rectangle[width + 2, height + 1];

            for (var i = 0; i < height + 1; i++) // +1 each side for the walls
            {
                field[0, i] = NewRectangle(Colors.DimGray);
                field[width + 1, i] = NewRectangle(Colors.DimGray);
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

            for (var i = 0; i < Figure.Width; i++)
            for (var j = 0; j < Figure.Height; j++)
            {
                if (Figure.Tiles[i, j] != null)
                    rectangles.Add(Figure.Tiles[i, j]);
            }

            for (var i = Field.GetLowerBound(0); i <= Field.GetUpperBound(0); i++)
            {
                for (var j = Field.GetLowerBound(1); j <= Field.GetUpperBound(1); j++)
                {
                    if (Field[i, j] != null)
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

        private void MoveLeft()
        {
            FigurePositionX--;

            if (!Check(FigurePositionX, FigurePositionY))
            {
                FigurePositionX++;
                return;
            }

            Figure.Draw(FigurePositionX * FieldHelper.BlockWidth, FigurePositionY * FieldHelper.BlockHeight);
        }

        private void MoveRight()
        {
            FigurePositionX++;

            if (!Check(FigurePositionX, FigurePositionY))
            {
                FigurePositionX--;
                return;
            }

            Figure.Draw(FigurePositionX * FieldHelper.BlockWidth, FigurePositionY * FieldHelper.BlockHeight);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MoveLeft();
        }

        private void RotateAntiClockWise()
        {
            Figure.RotateLeft();
            if (!Check(FigurePositionX, FigurePositionY))
            {
                Figure.RotateRight();
            }
            Figure.Draw(FigurePositionX * FieldHelper.BlockWidth, FigurePositionY * FieldHelper.BlockHeight);
        }

        private void RotateClockWise()
        {
            Figure.RotateRight();
            if (!Check(FigurePositionX, FigurePositionY))
            {
                Figure.RotateLeft();
            }
            Figure.Draw(FigurePositionX * FieldHelper.BlockWidth, FigurePositionY * FieldHelper.BlockHeight);
        }

        private new void Drop()
        {
            Game.IsFalling = true;
        }
        
        public bool Check(int x, int y)
        {
            for (var i = x; i < x + Figure.Width; i++)
            {
                for (var j = y; j < y + Figure.Height; j++)
                {
                    var r = Figure.Tiles[i - x, j - y];
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

        public static MainWindow Instance;

        public void SetRandomFigure()
        {
            Figure = new Figure();

            var r = new Random();
            switch (r.Next(1, 5))
            {
                case 1:
                    Figure.Tiles = new Rectangle[,]
                    {
                        {null, NewRectangle(Colors.Red), NewRectangle(Colors.Red), null},
                        {null, null, NewRectangle(Colors.Red), null},
                        {null, null, NewRectangle(Colors.Red), null},
                        {null, null, null, null}
                    };
                    break;
                case 2:
                    Figure.Tiles = new Rectangle[,]
                    {
                        {null, NewRectangle(Colors.LawnGreen), NewRectangle(Colors.LawnGreen), null},
                        {null, NewRectangle(Colors.LawnGreen), null, null},
                        {null, NewRectangle(Colors.LawnGreen), null, null},
                        {null, null, null, null}
                    };
                    break;
                case 3:
                    Figure.Tiles = new Rectangle[,]
                    {
                        {null, null, null, null},
                        {null, NewRectangle(Colors.Brown), NewRectangle(Colors.Brown), null},
                        {null, NewRectangle(Colors.Brown), NewRectangle(Colors.Brown), null},
                        {null, null, null, null}
                    };
                    break;
                case 4:
                    Figure.Tiles = new Rectangle[,]
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
            LineCountTextBlock.Text = Game.Lines.ToString(CultureInfo.InvariantCulture);

            if (!FieldHelper.CheckFigure(Field, Figure, FigurePositionX, FigurePositionY))
            {
                _timer.Stop();
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
                MoveLeft();
            }
            else if (e.Key == Key.Right)
            {
                MoveRight();
            }
            else if (e.Key == Key.Up)
            {
                RotateAntiClockWise();
            }
            else if (e.Key == Key.Down)
            {
                RotateClockWise();
            }

            else if (e.Key == Key.Space)
            {
                Drop();
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            RotateAntiClockWise();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            RotateClockWise();
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            MoveRight();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            Drop();
        }
    }
}
