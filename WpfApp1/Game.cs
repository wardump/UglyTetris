
using System.Windows.Controls;

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
}