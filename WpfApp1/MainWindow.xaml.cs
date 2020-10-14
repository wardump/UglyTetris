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

            Game.Figure.Tiles = new Tile[,]
            {
                {null, new Tile(Colors.Red), new Tile(Colors.Red), null},
                {null, null, new Tile(Colors.Red), null},
                {null, null, new Tile(Colors.Red), null},
                {null, null, null, null}
            };
            
            Game.Field = CreateField(FieldHelper.FieldDefaultWidth, FieldHelper.FieldDefaultHeight);

            Game.FigureChange += GameOnFigureChange;

            RedrawField(true, true);

            _timer = new System.Windows.Threading.DispatcherTimer {Interval = TimeSpan.FromMilliseconds(10)};

            _timer.Tick += (sender, args) => { Game.Tick(); };

            _timer.Start();
        }

        private void GameOnFigureChange(object? sender, EventArgs e)
        {
            RedrawField(false, true);
        }

        private readonly Dictionary<Tile, Rectangle> _tileRectangleMap = new Dictionary<Tile, Rectangle>();

        public Game Game = new Game();
        private readonly System.Windows.Threading.DispatcherTimer _timer;


        private Tile[,] CreateField(int width, int height)
        {
            var field = new Tile[width + 2, height + 1];

            for (var i = 0; i < height + 1; i++) // +1 each side for the walls
            {
                field[0, i] = new Tile(Colors.DimGray);
                field[width + 1, i] = new Tile(Colors.DimGray);
            }

            for (var i = 1; i < width + 1; i++)
            {
                field[i, height] = new Tile(Colors.DimGray);
            }

            return field;
        }

        private Rectangle RedrawTile(Tile tile, int x, int y)
        {
            Rectangle rectangle;

            if (_tileRectangleMap.ContainsKey(tile))
            {
                rectangle = _tileRectangleMap[tile];
            }
            else
            {
                rectangle = NewRectangle(tile.Color);
                _tileRectangleMap.Add(tile, rectangle);
            }

            Canvas.SetLeft(rectangle, x * FieldHelper.BlockWidth + 1);
            Canvas.SetTop(rectangle, y * FieldHelper.BlockHeight + 1);

            return rectangle;
        }
        
        private void RedrawField(bool redrawField, bool redrawFigure)
        {
            var tiles = new HashSet<Tile>();

            for (var i = 0; i < Game.Figure.Width; i++)
            for (var j = 0; j < Game.Figure.Height; j++)
            {
                var tile = Game.Figure.Tiles[i, j];
                if (tile != null)
                {
                    if (redrawFigure)
                    {
                        RedrawTile(tile, Game.FigurePositionX + i, Game.FigurePositionY + j);
                    }

                    tiles.Add(tile);
                }
            }

            for (var i = Game.Field.GetLowerBound(0); i <= Game.Field.GetUpperBound(0); i++)
            {
                for (var j = Game.Field.GetLowerBound(1); j <= Game.Field.GetUpperBound(1); j++)
                {
                    var tile = Game.Field[i, j];
                    if (tile != null)
                    {
                        if (redrawField)
                        {
                            RedrawTile(tile, i, j);
                        }

                        tiles.Add(tile);
                    }
                }
            }

            var orphanedTiles = _tileRectangleMap.Keys.Where(t => !tiles.Contains(t)).ToList();

            foreach (var orphanedTile in orphanedTiles)
            {
                var rectangle = _tileRectangleMap[orphanedTile];
                MainCanvas.Children.Remove(rectangle);
                _tileRectangleMap.Remove(orphanedTile);
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
            RedrawField(true, true);
            LineCountTextBlock.Text = Game.Lines.ToString(CultureInfo.InvariantCulture);

            if (!Game.ResetFigure(Figure.CreateRandomFigure()))
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
