using System.Collections.Generic;
using UglyTetris.GameLogic;

namespace UglyTetris.AvaloniaGUI
{
    internal class FigureDrawer : IDrawFigure
    {
        public void DrawFigure(Figure figure, int x, int y)
        {
            var tiles = new List<TileXy>();

            for(var i = 0; i < figure.Width; i++)
            for (var j = 0; j < figure.Height; j++)
            {
                var tile = figure.Tiles[i, j];
                if (tile != null)
                {
                    tiles.Add(new TileXy(){Tile = tile, X = x + i, Y = y+j});
                }
            }

            _tileDrawer.DrawTiles(tiles);
        }

        private readonly TileDrawer _tileDrawer;

        


        public FigureDrawer(TileDrawer tileDrawer)
        {
            _tileDrawer = tileDrawer;
        }
    }
}