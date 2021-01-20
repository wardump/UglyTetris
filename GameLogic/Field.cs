using System;
using System.Collections.Generic;

namespace UglyTetris.GameLogic
{
    public class Field
    {
        public int Xmin => _tiles.GetLowerBound(0);
        public int Xmax => _tiles.GetUpperBound(0);
        public int Width => Xmax - Xmin;
        public int Ymin => _tiles.GetLowerBound(1);
        public int Ymax => _tiles.GetUpperBound(1);
        public int Height => Ymax - Ymin;
        
        public Field(Tile[,] initialTiles)
        {
            _tiles = initialTiles;
        }
        
        public static Field CreateField(int width, int height, string color)
        {
            var tiles = new Tile[width + 2, height + 1];

            for (var i = 0; i < height + 1; i++) // +1 each side for the walls
            {
                tiles[0, i] = new Tile(color);
                tiles[width + 1, i] = new Tile(color);
            }

            for (var i = 1; i < width + 1; i++)
            {
                tiles[i, height] = new Tile("DimGray");
            }

            return new Field(tiles);
        }

        private bool IsInBounds(int x, int y)
        {
            return x >= Xmin && x <= Xmax && y >= Ymin && y <= Ymax;
        }

        public bool IsEmpty(int x, int y)
        {
            return IsInBounds(x, y) && _tiles[x, y] == null;
        }

        private Tile GetTile(int x, int y)
        {
            return IsInBounds(x, y) ? _tiles[x, y] : null;
        }

        public IEnumerable<TileXy> GetTiles()
        {
            for (var i = Xmin; i <= Xmax; i++)
            {
                for (var j = Ymin; j <= Ymax; j++)
                {
                    var tile = GetTile(i, j);
                    if (tile != null)
                    {
                        yield return new TileXy {Tile = tile, X = i, Y = j};
                    }
                }
            }
        }

        public bool SetTile(int x, int y, Tile tile)
        {
            if (!IsInBounds(x, y))
            {
                throw new ArgumentOutOfRangeException("Coordinates are out of bounds");
            }

            var oldTile = _tiles[x, y];

            if (oldTile == tile)
            {
                return false;
            }
            
            _tiles[x, y] = tile;
            var tileXyOld = new TileXy {Tile = oldTile, X = x, Y = y};
            var tileXyNew = new TileXy {Tile = tile, X = x, Y = y};

            RaiseTileChanged(tileXyOld, tileXyNew);

            return true;
        }

        private void MoveTile(int x, int y, int newX, int newY)
        {
            var replacedTile = GetTile(newX, newY);

            if (replacedTile != null)
            {
                SetTile(newX, newY, null);
            }

            var movingTile = GetTile(x, y);

            if (movingTile == null)
            {
                return;
            }
            
            _tiles[newX, newY] = movingTile;
            _tiles[x, y] = null;

            var oldTileXy = new TileXy() {Tile = movingTile, X = x, Y = y};
            var newTileXy = new TileXy() {Tile = movingTile, X = newX, Y = newY};

            RaiseTileChanged(oldTileXy, newTileXy);
        }

        public event EventHandler<TileChangedEventArgs> TileChanged;

        private void RaiseTileChanged(TileXy oldTile, TileXy newTile)
        {
            TileChanged?.Invoke(this, new TileChangedEventArgs(oldTile, newTile));
        }
        
        
        /// <summary>
        /// Removes full lines from the field, shifts down the field tiles above the removed lines.
        /// </summary>
        /// <returns>The number of removed lines</returns>
        public int RemoveFullLines() // !note that the old name 'CheckLines' was bad
        {
            // this code assumes that the left and right columns and the bottom row 
            // are the walls, and the game area is inside of it
            
            //todo better have specific methods for getting game area
            // which can be extended in the future
            
            var left = Xmin + 1;
            var right = Xmax - 1;

            var remove = 0;

            for (var i = Ymin; i < Ymax; i++)
            {
                var isFull = true;
                
                for (var x = left; x <= right ; x++) // check line
                {
                    if (_tiles[x, i] != null) continue;
                    
                    isFull = false;
                    break;
                }

                if (!isFull) continue;
                
                // remove the line
                remove++;

                for (var x = left; x <= right; x++)
                {
                    for (var y = i - 1; y >= 0; y--)
                    {
                        MoveTile(x, y, x, y + 1);
                    }
                }
                
            }

            return remove;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="figure"></param>
        /// <param name="figureX"></param>
        /// <param name="figureY"></param>
        /// <returns></returns>
        public bool IsPossibleToPlaceFigure(Figure figure, int figureX, int figureY)
        {
            // this code assumes that the left and right columns and the bottom row 
            // are the walls, and the game area is inside of it
            
            //todo better have specific methods for getting game area
            // which can be extended in the future
            
            var left = Xmin + 1;
            var right = Xmax - 1;
            var bottom = Ymax - 1;

            for (var i = figure.Tiles.GetLowerBound(0); i <= figure.Tiles.GetUpperBound(0); i++)
            {
                for (var j = figure.Tiles.GetLowerBound(1); j <= figure.Tiles.GetUpperBound(1); j++)
                {
                    if (figure.Tiles[i, j] == null) continue;
                    
                    var figureTileX = figureX + i;
                    var figureTileY = figureY + j;

                    if (!IsEmpty(figureTileX, figureTileY))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void LockFigure(Figure figure, int x, int y, bool removeTilesFromFigure)
        {
            for (var i = figure.Tiles.GetLowerBound(0); i <= figure.Tiles.GetUpperBound(0); i++)
            {
                for (var j = figure.Tiles.GetLowerBound(1); j <= figure.Tiles.GetUpperBound(1); j++)
                {
                    if (figure.Tiles[i, j] == null) continue;
                    
                    SetTile(x + i, y + j, figure.Tiles[i, j]);

                    if (removeTilesFromFigure)
                    {
                        figure.Tiles[i, j] = null;
                    }
                }
            }
        }
        
        private Tile[,] _tiles;
    }
}