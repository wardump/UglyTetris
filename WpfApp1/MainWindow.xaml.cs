using System;
using System.Globalization;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using UglyTetris.GameLogic;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            _figureDrawer = new FigureDrawer(new TileDrawer(MainCanvas));
            _fieldDrawer = new FieldDrawer(new TileDrawer(MainCanvas));

            Game = new Game(new RandomNextFigureFactory());
            Game.FigureStateChanged += GameOnFigureStateChanged;
            Game.LinesChanged += GameOnLinesChanged;
            Game.StateChanged += GameOnStateChanged;
            
            Game.Field = Field.CreateField(FieldHelper.FieldDefaultWidth, FieldHelper.FieldDefaultHeight, "DimGray");
            Game.ResetFigure(_figureFactory.CreateRandomFigure());

            _figureDrawer.DrawFigure(Game.Figure, Game.FigurePositionX, Game.FigurePositionY);
            _fieldDrawer.AttachToField(Game.Field);


            _timer = new System.Windows.Threading.DispatcherTimer {Interval = TimeSpan.FromMilliseconds(10)};

            _timer.Tick += (sender, args) => { Game.Tick(); };

            _timer.Start();
        }

        private void GameOnStateChanged(object sender, EventArgs e)
        {
            if (Game.State == GameState.GameOver)
            {
                _timer.Stop();
                MessageBox.Show("GAME OVER");
            }
        }

        private void GameOnLinesChanged(object sender, EventArgs e)
        {
            LineCountTextBlock.Text = Game.Lines.ToString(CultureInfo.InvariantCulture);
        }

        private void GameOnFigureStateChanged(object sender, EventArgs e)
        {
            _figureDrawer.DrawFigure(Game.Figure, Game.FigurePositionX, Game.FigurePositionY);
        }


        public Game Game;
        private readonly System.Windows.Threading.DispatcherTimer _timer;

        private FieldDrawer _fieldDrawer;
        private FigureDrawer _figureDrawer;

        
        

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
        
        private FigureFactory _figureFactory = new FigureFactory();

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

    internal class RandomNextFigureFactory : INextFigureFactory
    {
        public Figure GetNextFigure()
        {
            return _figureFactory.CreateRandomFigure();
        }

        readonly FigureFactory _figureFactory = new FigureFactory();
    }
}
