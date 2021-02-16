using System;
using System.Globalization;
using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Threading;
using UglyTetris.GameLogic;

namespace UglyTetris.AvaloniaGUI
{
    public class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var mainCanvas = this.FindControl<Canvas>("MainCanvas");
            
            _figureDrawer = new FigureDrawer(new TileDrawer(mainCanvas));
            _fieldDrawer = new FieldDrawer(new TileDrawer(mainCanvas));

            Game = new Game(new RandomNextFigureFactory());
            Game.FigureStateChanged += GameOnFigureStateChanged;
            Game.LinesChanged += GameOnLinesChanged;
            Game.StateChanged += GameOnStateChanged;
            
            Game.Field = Field.CreateField(FieldHelper.FieldDefaultWidth, FieldHelper.FieldDefaultHeight, "DimGray");
            Game.ResetFigure(_figureFactory.CreateRandomFigure());

            _figureDrawer.DrawFigure(Game.Figure, Game.FigurePositionX, Game.FigurePositionY);
            _fieldDrawer.AttachToField(Game.Field);


            _timer = new DispatcherTimer {Interval = TimeSpan.FromMilliseconds(10)};

            _timer.Tick += (sender, args) => { Game.Tick(); };

            _timer.Start();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
        
        private void GameOnStateChanged(object sender, EventArgs e)
        {
            if (Game.State == GameState.GameOver)
            {
                _timer.Stop();
                
                var gameOverTextBlock = this.FindControl<TextBlock>("LineCountTextBlock");
                gameOverTextBlock.IsVisible = true; //todo this does not work
            }
        }

        private void GameOnLinesChanged(object sender, EventArgs e)
        {
            var lineCountTextBlock = this.FindControl<TextBlock>("LineCountTextBlock");
            lineCountTextBlock.Text = Game.Lines.ToString(CultureInfo.InvariantCulture);
        }

        private void GameOnFigureStateChanged(object sender, EventArgs e)
        {
            _figureDrawer.DrawFigure(Game.Figure, Game.FigurePositionX, Game.FigurePositionY);
        }


        public Game Game;
        private readonly DispatcherTimer _timer;

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

        private void Drop()
        {
            Game.Drop();
        }

        private void CaptureFigure()
        {
            Game.CaptureFigure();
        }

        private void DismissFigure()
        {
            Game.DismissFigure();
        }

        private FigureFactory _figureFactory = new FigureFactory();

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
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
            else if (e.Key == Key.C)
            {
                CaptureFigure();
            }
            else if (e.Key == Key.D)
            {
                DismissFigure();
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