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

        public void DrawField(Field field)
        {
            var tiles = new List<TileXy>();

            for (var i = field.Xmin; i <= field.Xmax; i++)
            {
                for (var j = field.Ymin; j <= field.Ymax; j++)
                {
                    var tile = field.GetTile(i, j);
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