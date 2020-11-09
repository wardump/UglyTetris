
using System;
using System.Windows.Shapes;

namespace WpfApp1
{
    public class Game
    {
        public bool IsFalling { get; set; }

        private int _tickCount = 0;

        public int MoveDownPeriodTicks = 50;

        public int FallDownPeriodTicks = 3;

        public int Lines = 0;

        public void Tick()
        {
            _tickCount++;

            bool moveDown = IsFalling
                ? (_tickCount % FallDownPeriodTicks == 0)
                : (_tickCount % MoveDownPeriodTicks == 0);


            if (moveDown)
            {
                var y = FigurePositionY + 1;
                var x = FigurePositionX;

                var w = MainWindow.Instance;

                if (!Field.IsPossibleToPlaceFigure(Figure, x, y))
                {
                    Field.LockFigure(Figure, FigurePositionX, FigurePositionY, true);
                    
                    var lineCount = Field.RemoveFullLines();
                    Lines += lineCount;

                    RaiseFigureStateChanged();
                    w.OnFigureLock();
                    
                    _tickCount = 0;
                    IsFalling = false;
                }
                else
                {
                    FigurePositionX = x;
                    FigurePositionY = y;
                    RaiseFigureStateChanged();
                }
            }
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

        protected void RaiseFigureStateChanged()
        {
            FigureStateChanged?.Invoke(this, EventArgs.Empty);
        }

        public int FigurePositionX { get; private set; } = 6;

        public int FigurePositionY { get; private set; } = 0;

        public Field Field;

        public bool ResetFigure(Figure newFigure)
        {
            FigurePositionX = (Field.Xmax - Field.Xmin) / 2;
            FigurePositionY = 0;

            if (Field.IsPossibleToPlaceFigure(newFigure, FigurePositionX, FigurePositionY))
            {
                Figure = newFigure;
                return true;
            }

            return false; //cannot reset figure
        }
    }
}