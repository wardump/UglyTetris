using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace WpfApp1
{
    public class Figure
    {
        public Rectangle[,] Tiles = new Rectangle[,]
        {
            {null, null, null, null},
            {null, null, null, null},
            {null, null, null, null},
            {null, null, null, null}
        };

        public void Draw(double left, double top)
        {
            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    if (Tiles[i, j] != null)
                    {
                        Canvas.SetLeft(Tiles[i, j], left + i * 20 + 1);
                        Canvas.SetTop(Tiles[i, j], top + j * 20 + 1);
                    }
                }
            }
        }

        public void RotateLeft()
        {
            var newTiles = new Rectangle[4, 4];

            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    newTiles[j, 3 - i] = Tiles[i, j];
                }
            }

            Tiles = newTiles;
        }

        public void RotateRight()
        {
            var newTiles = new Rectangle[4, 4];

            for (var i = 0; i < 4; i++)
            {
                for (var j = 0; j < 4; j++)
                {
                    newTiles[3 - j, i] = Tiles[i, j];
                }
            }

            Tiles = newTiles;
        }
    }
}