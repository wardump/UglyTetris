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
            
            _figureDrawer = new FigureDrawer(new TileDrawer(MainCanvas));
            _fieldDrawer = new FieldDrawer(new TileDrawer(MainCanvas));

            Game = new Game();
            Game.FigureStateChanged += GameOnFigureStateChanged;
            
            Game.Field = CreateField(FieldHelper.FieldDefaultWidth, FieldHelper.FieldDefaultHeight);
            Game.ResetFigure(_figureFactory.CreateRandomFigure());

            _figureDrawer.DrawFigure(Game.Figure, Game.FigurePositionX, Game.FigurePositionY);
            _fieldDrawer.DrawField(Game.Field);


            _timer = new System.Windows.Threading.DispatcherTimer {Interval = TimeSpan.FromMilliseconds(10)};

            _timer.Tick += (sender, args) => { Game.Tick(); };

            _timer.Start();
        }

        private void GameOnFigureStateChanged(object? sender, EventArgs e)
        {
            _figureDrawer.DrawFigure(Game.Figure, Game.FigurePositionX, Game.FigurePositionY);
        }


        public Game Game;
        private readonly System.Windows.Threading.DispatcherTimer _timer;

        private FieldDrawer _fieldDrawer;
        private FigureDrawer _figureDrawer;

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
        
        public static MainWindow Instance;

        private FigureFactory _figureFactory = new FigureFactory();

        public void OnFigureLock()
        {
            _fieldDrawer.DrawField(Game.Field);
            LineCountTextBlock.Text = Game.Lines.ToString(CultureInfo.InvariantCulture);

            var figure = _figureFactory.CreateRandomFigure();

            if (!Game.ResetFigure(figure))
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
