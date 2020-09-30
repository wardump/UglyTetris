using System.Windows.Shapes;

namespace WpfApp1
{
    public static class FieldHelper
    {
        public static double BlockWidth = 20;
        public static double BlockHeight = 20;

        public static int FieldDefaultWidth = 12;
        public static int FieldDefaultHeight = 24;

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