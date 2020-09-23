
using System.Windows.Controls;
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
                var y = MainWindow.Instance.fy + 1;
                var x = MainWindow.Instance.fx;

                var w = MainWindow.Instance;

                if (!w.Check(x, y))
                {
                    var f = MainWindow.Instance.TestFigure;
                    for (var i = f.Tiles.GetLowerBound(0); i <= f.Tiles.GetUpperBound(0); i++)
                    {
                        for (var j = f.Tiles.GetLowerBound(1); j <= f.Tiles.GetUpperBound(1); j++)
                        {
                            if (f.Tiles[i, j] != null)
                            {
                                w.Field[w.fx + i, w.fy + j] = f.Tiles[i, j];
                            }
                        }
                    }

                    var lines = FieldHelper.CheckLines(w.Field);

                    Lines += lines;

                    w.fx = 6;
                    w.fy = 0;
                    w.SetRandomFigure();
                    
                    _tickCount = 0;
                    IsFalling = false;
                }
                else
                {
                    w.fx = x;
                    w.fy = y;
                    w.TestFigure.Draw(20 * x, 20 * y);
                }
            }
        }
    }

    public class FieldHelper
    {
        public static int CheckLines(Rectangle[,] field)
        {
            var left = field.GetLowerBound(0) + 1;
            var right = field.GetUpperBound(0) - 1;

            var remove = 0;

            for (var i = field.GetLowerBound(1); i < field.GetUpperBound(1); i++)
            {
                var isFull = true;
                for (var x = left; x <= right ; x++) // check line
                {
                    if (field[x, i] == null)
                    {
                        isFull = false;
                        break;
                    }
                }

                if (isFull)
                {
                    // remove the line
                    remove++;

                    for (var x = left; x <= right; x++)
                    {
                        for (var y = i - 1; y >= 0; y--)
                        {
                            field[x, y + 1] = field[x, y];
                        }
                    }
                }
            }

            return remove;
        }

        public static bool CheckFigure(Rectangle[,] field, Figure f, int figureX, int figureY)
        {
            var left = field.GetLowerBound(0) + 1;
            var right = field.GetUpperBound(0) - 1;
            var bottom = field.GetUpperBound(1) - 1;

            for (var i = f.Tiles.GetLowerBound(0); i <= f.Tiles.GetUpperBound(0); i++)
            {
                for (var j = f.Tiles.GetLowerBound(1); j <= f.Tiles.GetUpperBound(1); j++)
                {
                    if (f.Tiles[i, j] != null)
                    {
                        if (figureX + i > right || figureX + i < 0 || figureY + j > bottom || figureY + j < 0)
                        {
                            return false;
                        }

                        if (field[figureX + i, figureY + j] != null)
                        {
                            return false;
                        }
                    }
                }
            }

            return true;
        }
    }
}