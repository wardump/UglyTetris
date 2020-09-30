
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
                var y = MainWindow.Instance.FigurePositionY + 1;
                var x = MainWindow.Instance.FigurePositionX;

                var w = MainWindow.Instance;

                if (!w.Check(x, y))
                {
                    var f = MainWindow.Instance.Figure;
                    for (var i = f.Tiles.GetLowerBound(0); i <= f.Tiles.GetUpperBound(0); i++)
                    {
                        for (var j = f.Tiles.GetLowerBound(1); j <= f.Tiles.GetUpperBound(1); j++)
                        {
                            if (f.Tiles[i, j] != null)
                            {
                                w.Field[w.FigurePositionX + i, w.FigurePositionY + j] = f.Tiles[i, j];
                            }
                        }
                    }

                    var lines = FieldHelper.CheckLines(w.Field);

                    Lines += lines;

                    w.FigurePositionX = 6;
                    w.FigurePositionY = 0;
                    w.SetRandomFigure();
                    
                    _tickCount = 0;
                    IsFalling = false;
                }
                else
                {
                    w.FigurePositionX = x;
                    w.FigurePositionY = y;
                    w.Figure.Draw(FieldHelper.BlockWidth * x, FieldHelper.BlockHeight * y);
                }
            }
        }
    }
}