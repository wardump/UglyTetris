using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    internal class TileDrawer
    {
        public TileDrawer(Canvas canvas)
        {
            _canvas = canvas;
        }

        public void DrawTiles(IEnumerable<TileXy> tiles)
        {
            var affectedTiles = new HashSet<Tile>();

            foreach (var tileXy in tiles)
            {
                DrawTile(tileXy);
                affectedTiles.Add(tileXy.Tile);
            }

            var orphanTiles = _tileRectangleMap.Keys.Where(t => !affectedTiles.Contains(t)).ToList();
            foreach (var orphanTile in orphanTiles)
            {
                _canvas.Children.Remove(_tileRectangleMap[orphanTile]);
                _tileRectangleMap.Remove(orphanTile);
            }
        }

        private Rectangle DrawTile(TileXy tileXy)
        {
            Rectangle rectangle;

            if (_tileRectangleMap.ContainsKey(tileXy.Tile))
            {
                rectangle = _tileRectangleMap[tileXy.Tile];
            }
            else
            {
                rectangle = NewRectangle(tileXy.Tile.Color);
                _tileRectangleMap.Add(tileXy.Tile, rectangle);
            }

            Canvas.SetLeft(rectangle, tileXy.X * FieldHelper.BlockWidth + 1);
            Canvas.SetTop(rectangle, tileXy.Y * FieldHelper.BlockHeight + 1);

            return rectangle;
        }

        private Rectangle NewRectangle(Color color)
        {
            var r = new Rectangle
            {
                Width = FieldHelper.BlockWidth * 0.9,
                Height = FieldHelper.BlockHeight * 0.9,
                Fill = new SolidColorBrush(color)
            };
            _canvas.Children.Add(r);
            return r;
        }

        private readonly Dictionary<Tile, Rectangle> _tileRectangleMap = new Dictionary<Tile, Rectangle>();
        private readonly Canvas _canvas;
    }
}