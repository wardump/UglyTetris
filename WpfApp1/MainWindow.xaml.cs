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

            Game.Figure.Tiles = new Rectangle[,]
            {
                {null, NewRectangle(Colors.Red), NewRectangle(Colors.Red), null},
                {null, null, NewRectangle(Colors.Red), null},
                {null, null, NewRectangle(Colors.Red), null},
                {null, null, null, null}
            };

            Game.Figure.Draw(Game.FigurePositionX * FieldHelper.BlockWidth, Game.FigurePositionY * FieldHelper.BlockHeight);

            Game.Field = CreateField(FieldHelper.FieldDefaultWidth, FieldHelper.FieldDefaultHeight);
            RedrawField();

            _timer = new System.Windows.Threading.DispatcherTimer {Interval = TimeSpan.FromMilliseconds(10)};

            _timer.Tick += (sender, args) => { Game.Tick(); };

            _timer.Start();
        }



        public Game Game = new Game();
        private System.Windows.Threading.DispatcherTimer _timer;


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

            for (var i = 0; i < Game.Figure.Width; i++)
            for (var j = 0; j < Game.Figure.Height; j++)
            {
                if (Game.Figure.Tiles[i, j] != null)
                    rectangles.Add(Game.Figure.Tiles[i, j]);
            }

            for (var i = Game.Field.GetLowerBound(0); i <= Game.Field.GetUpperBound(0); i++)
            {
                for (var j = Game.Field.GetLowerBound(1); j <= Game.Field.GetUpperBound(1); j++)
                {
                    if (Game.Field[i, j] != null)
                    {
                        rectangles.Add(Game.Field[i, j]);
                        Canvas.SetLeft(Game.Field[i, j], i * FieldHelper.BlockWidth + 1);
                        Canvas.SetTop(Game.Field[i, j], j * FieldHelper.BlockHeight + 1);
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
            Game.MoveLeft();
        }

        private void MoveRight()
        {
            Game.MoveRight();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MoveLeft();
        }

        private void RotateAntiClockWise()
        {
            Game.RotateAntiClockWise();
        }

        private void RotateClockWise()
        {
            Game.RotateClockWise();
        }

        private new void Drop()
        {
            Game.Drop();
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
        

        public void OnFigureLock()
        {
            RedrawField();
            LineCountTextBlock.Text = Game.Lines.ToString(CultureInfo.InvariantCulture);

            if (!Game.ResetFigure(Figure.CreateRandomFigure(NewRectangle)))
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
