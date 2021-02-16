using System;

namespace UglyTetris.GameLogic
{
    public class Game
    {
        public Game(INextFigureFactory nextFigureFactory)
        {
            _nextFigureFactory = nextFigureFactory;
        }

        private bool IsFalling { get; set; }

        private int _tickCount = 0;
        private int MoveDownPeriodTicks { get; } = 50;
        private int FallDownPeriodTicks { get; } = 3;
        
        private GameState _state = GameState.Running;
        
        private int _lines = 0;
        public int Lines
        {
            get => _lines;
            private set
            {
                _lines = value;
                LinesChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        public GameState State
        {
            get => _state;
            private set
            {
                _state = value;
                StateChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public void Tick()
        {
            if (State == GameState.GameOver)
            {
                return;
            }
            
            _tickCount++;

            var moveDown = IsFalling
                ? (_tickCount % FallDownPeriodTicks == 0)
                : (_tickCount % MoveDownPeriodTicks == 0);


            if (!moveDown) return;
            
            var y = FigurePositionY + 1;
            var x = FigurePositionX;

            if (Field.IsPossibleToPlaceFigure(Figure, x, y)) {
                FigurePositionX = x;
                FigurePositionY = y;
                RaiseFigureStateChanged();
                return;
            }
            
            Field.LockFigure(Figure, FigurePositionX, FigurePositionY, true);
                    
            var lineCount = Field.RemoveFullLines();
            Lines += lineCount;

            RaiseFigureStateChanged();

            var figure = _nextFigureFactory.GetNextFigure();

            if (!ResetFigure(figure))
            {
                State = GameState.GameOver;
            }
                    
            _tickCount = 0;
            IsFalling = false;
        }

        public void MoveLeft()
        {
            FigurePositionX--;

            if (!Field.IsPossibleToPlaceFigure(Figure, FigurePositionX, FigurePositionY))
            {
                FigurePositionX++;
                return;
            }

            RaiseFigureStateChanged();
        }

        public void MoveRight()
        {
            FigurePositionX++;

            if (!Field.IsPossibleToPlaceFigure(Figure, FigurePositionX, FigurePositionY))
            {
                FigurePositionX--;
                return;
            }

            RaiseFigureStateChanged();
        }

        public void RotateAntiClockWise()
        {
            Figure.RotateLeft();
            if (!Field.IsPossibleToPlaceFigure(Figure, FigurePositionX, FigurePositionY))
            {
                Figure.RotateRight();
            }
            RaiseFigureStateChanged();
        }

        public void RotateClockWise()
        {
            Figure.RotateRight();
            if (!Field.IsPossibleToPlaceFigure(Figure, FigurePositionX, FigurePositionY))
            {
                Figure.RotateLeft();
            }
            RaiseFigureStateChanged();
        }

        public void Drop()
        {
            IsFalling = true;
        }

        public Figure Figure { get; private set; } = new Figure();

        public event EventHandler FigureStateChanged;

        private void RaiseFigureStateChanged()
        {
            FigureStateChanged?.Invoke(this, EventArgs.Empty);
        }
        
        public event EventHandler LinesChanged; // may be replaced with INotifyPropertyChanged interface implementation
        
        public event EventHandler StateChanged; // may be replaced with INotifyPropertyChanged interface implementation

        

        public int FigurePositionX { get; private set; } = 6;

        public int FigurePositionY { get; private set; } = 0;

        public Field Field;
        

        public bool ResetFigure(Figure newFigure)
        {
            FigurePositionX = (Field.Xmax - Field.Xmin) / 2;
            FigurePositionY = 0;

            if (!Field.IsPossibleToPlaceFigure(newFigure, FigurePositionX, FigurePositionY))
                return false; //cannot reset figure
            Figure = newFigure;
            return true;

        }

        private Figure CapturedFigure { get; set; } = null;

        public bool CaptureFigure()
        {
            if (CapturedFigure != null) return false;
            
            CapturedFigure = Figure;
            var nextFigure = _nextFigureFactory.GetNextFigure();
            ResetFigure(nextFigure);
            return true;
        }

        public bool DismissFigure()
        {
            if (CapturedFigure == null) return false;
            ResetFigure(CapturedFigure);
            CapturedFigure = null;
            return true;
        }

        private INextFigureFactory _nextFigureFactory;
    }
}