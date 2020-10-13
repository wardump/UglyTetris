
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
                    Figure.Draw(FieldHelper.BlockWidth * x, FieldHelper.BlockHeight * y);
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

            Figure.Draw(FigurePositionX * FieldHelper.BlockWidth, FigurePositionY * FieldHelper.BlockHeight);
        }

        public void MoveRight()
        {
            FigurePositionX++;

            if (!Check(FigurePositionX, FigurePositionY))
            {
                FigurePositionX--;
                return;
            }

            Figure.Draw(FigurePositionX * FieldHelper.BlockWidth, FigurePositionY * FieldHelper.BlockHeight);
        }

        public void RotateAntiClockWise()
        {
            Figure.RotateLeft();
            if (!Check(FigurePositionX, FigurePositionY))
            {
                Figure.RotateRight();
            }
            Figure.Draw(FigurePositionX * FieldHelper.BlockWidth, FigurePositionY * FieldHelper.BlockHeight);
        }

        public void RotateClockWise()
        {
            Figure.RotateRight();
            if (!Check(FigurePositionX, FigurePositionY))
            {
                Figure.RotateLeft();
            }
            Figure.Draw(FigurePositionX * FieldHelper.BlockWidth, FigurePositionY * FieldHelper.BlockHeight);
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
        
        public Figure Figure { get; private set; } = new Figure();
        public int FigurePositionX { get; private set; } = 6;
        public int FigurePositionY { get; private set; } = 0;

        public Tile[,] Field;

        public bool ResetFigure(Figure newFigure)
        {
            FigurePositionX = 6; //todo calculate from field size
            FigurePositionY = 0;

            if (FieldHelper.CheckFigure(Field, newFigure, FigurePositionX, FigurePositionY))
            {
                Figure = newFigure;
                return true;
            }

            return false; //cannot reset figure
        }
    }
}