using System;

namespace UglyTetris.GameLogic
{
    public class TileChangedEventArgs: EventArgs
    {
        public TileChangedEventArgs(TileXy oldTile, TileXy newTile)
        {
            OldTile = oldTile;
            NewTile = newTile;
        }

        /// Deleted tile
        public TileXy OldTile { get; set; }
        
        /// Added tile
        public TileXy NewTile { get; set; }

        public bool IsDelete()
        {
            return NewTile.Tile == null && OldTile.Tile != null;
        }

        public bool IsMove()
        {
            return OldTile.Tile == NewTile.Tile && OldTile.Tile != null;
        }

        public bool IsAdd()
        {
            return NewTile.Tile != null && OldTile.Tile == null;
        }
    }
}