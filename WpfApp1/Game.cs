
using System;
using System.Windows.Shapes;
using System.Xml.Serialization;

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

                if (!Check(x, y))
                {
                    var f = Figure;
                    for (var i = f.Tiles.GetLowerBound(0); i <= f.Tiles.GetUpperBound(0); i++)
                    {
                        for (var j = f.Tiles.GetLowerBound(1); j <= f.Tiles.GetUpperBound(1); j++)
                        {
                            if (f.Tiles[i, j] != null)
                            {
                                Field[FigurePositionX + i, FigurePositionY + j] = f.Tiles[i, j];
                                f.Tiles[i, j] = null;
                            }
                        }
                    }

                    var lines = FieldHelper.CheckLines(Field);
                    Lines += lines;
                    
                    w.OnFigureLock();
                    
                    _tickCount = 0;
                    IsFalling = false;
                }
                else
                {
                    FigurePositionX = x;
                    FigurePositionY = y;
                    FigureChange?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public void MoveLeft()
        {
            FigurePositionX--;

            if (!Check(FigurePositionX, FigurePositionY))
            {
                FigurePositionX++;
                return;
            }
            FigureChange?.Invoke(this, EventArgs.Empty);
        }

        public void MoveRight()
        {
            FigurePositionX++;

            if (!Check(FigurePositionX, FigurePositionY))
            {
                FigurePositionX--;
                return;
            }
            FigureChange?.Invoke(this, EventArgs.Empty);
        }

        public void RotateAntiClockWise()
        {
            Figure.RotateLeft();
            if (!Check(FigurePositionX, FigurePositionY))
            {
                Figure.RotateRight();
            }
            FigureChange?.Invoke(this, EventArgs.Empty);
        }

        public void RotateClockWise()
        {
            Figure.RotateRight();
            if (!Check(FigurePositionX, FigurePositionY))
            {
                Figure.RotateLeft();
            }
            FigureChange?.Invoke(this, EventArgs.Empty);
        }

        public void Drop()
        {
            IsFalling = true;
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

        public event EventHandler FigureChange;

        public Figure Figure { get; private set; } = new Figure();
        public int FigurePositionX { get; private set; } = 6;
        public int FigurePositionY { get; private set; } = 0;

        public Tile[,] Field;

        public bool ResetFigure(Figure newFigure)
        {
            (int x, int y) = GetFigureStartPosition();

            if (FieldHelper.CheckFigure(Field, newFigure, x, y))
            {
                Figure = newFigure;

                FigurePositionX = x;
                FigurePositionY = y;

                FigureChange?.Invoke(this, EventArgs.Empty);

                return true;
            }

            return false; //cannot reset figure
        }

        private (int x, int y) GetFigureStartPosition()
        {
            return (FieldHelper.FieldDefaultWidth / 2, 0);
        }
    }
}