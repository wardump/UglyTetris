using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public void AttachToField(Field field)
        {
            if (_field != null)
            {
                DetachFromField();
            }

            _field = field;
            DrawField(field);
            
            field.TileChanged += FieldOnTileChanged;
        }

        public void DetachFromField()
        {
            if (_field != null)
            {
                _field.TileChanged -= FieldOnTileChanged;
            }
            
            _tileDrawer.DrawTiles(Enumerable.Empty<TileXy>());
        }

        private void FieldOnTileChanged(object sender, TileChangedEventArgs e)
        {
            if (e.IsMove() || e.IsAdd())
            {
                _tileDrawer.DrawTile(e.NewTile);
            }
            else if (e.IsDelete())
            {
                _tileDrawer.RemoveTile(e.OldTile.Tile);
            }
            else
            {
                _tileDrawer.RemoveTile(e.OldTile.Tile);
                _tileDrawer.DrawTile(e.NewTile);
            }
        }

        public void DrawField(Field field)
        {
            _tileDrawer.DrawTiles(field.GetTiles());
        }

        private readonly TileDrawer _tileDrawer;
        private Field _field;
    }
}