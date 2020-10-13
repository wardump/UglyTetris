using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WpfApp1
{
    internal class FieldDrawer
    {
        public FieldDrawer(TileDrawer tileDrawer)
        {
            _tileDrawer = tileDrawer;
        }

        public void DrawField(Tile[,] field)
        {
             var tiles = new List<TileXy>();

            for (var i = field.GetLowerBound(0); i <= field.GetUpperBound(0); i++)
            {
                for (var j = field.GetLowerBound(1); j <= field.GetUpperBound(1); j++)
                {
                    var tile = field[i, j];
                    if (tile != null)
                    {
                        tiles.Add(new TileXy() {Tile = tile, X = i, Y = j});
                    }
                }
            }

            _tileDrawer.DrawTiles(tiles);
        }

        private readonly TileDrawer _tileDrawer;
    }
}