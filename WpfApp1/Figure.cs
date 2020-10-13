using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace WpfApp1
{
    public class Figure
    {
        public Tile[,] Tiles = new Tile[,]
        {
            {null, null, null, null},
            {null, null, null, null},
            {null, null, null, null},
            {null, null, null, null}
        };

        public int Width => Tiles.GetUpperBound(0) + 1;
        public int Height => Tiles.GetUpperBound(1) + 1;

        public void Draw(double left, double top)
        {
            //TODO

            //for (var i = 0; i < Width; i++)
            //{
            //    for (var j = 0; j < Height; j++)
            //    {
            //        if (Tiles[i, j] != null)
            //        {
            //            Canvas.SetLeft(Tiles[i, j], left + i * FieldHelper.BlockWidth + 1);
            //            Canvas.SetTop(Tiles[i, j], top + j * FieldHelper.BlockHeight + 1);
            //        }
            //    }
            //}
        }

        public void RotateLeft()
        {
            var newTiles = new Tile[Height, Width]; // rotation swaps coordinates

            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    newTiles[j, Width - i - 1] = Tiles[i, j];
                }
            }

            Tiles = newTiles;
        }

        public void RotateRight()
        {
            var newTiles = new Tile[Height, Width]; // rotation swaps coordinates

            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    newTiles[Height - j - 1, i] = Tiles[i, j];
                }
            }

            Tiles = newTiles;
        }

        public static Figure CreateRandomFigure()
        {
            var r = new Random();
            var figure = new Figure();
            switch (r.Next(1, 5))
            {
                case 1:
                    figure.Tiles = new Tile[,]
                    {
                        {null, new Tile(Colors.Red), new Tile(Colors.Red), null},
                        {null, null, new Tile(Colors.Red), null},
                        {null, null, new Tile(Colors.Red), null},
                        {null, null, null, null}
                    };
                    break;
                case 2:
                    figure.Tiles = new Tile[,]
                    {
                        {null, new Tile(Colors.LawnGreen), new Tile(Colors.LawnGreen), null},
                        {null, new Tile(Colors.LawnGreen), null, null},
                        {null, new Tile(Colors.LawnGreen), null, null},
                        {null, null, null, null}
                    };
                    break;
                case 3:
                    figure.Tiles = new Tile[,]
                    {
                        {null, null, null, null},
                        {null, new Tile(Colors.Brown), new Tile(Colors.Brown), null},
                        {null, new Tile(Colors.Brown), new Tile(Colors.Brown), null},
                        {null, null, null, null}
                    };
                    break;
                case 4:
                    figure.Tiles = new Tile[,]
                    {
                        {null, new Tile(Colors.DeepSkyBlue), null, null},
                        {null, new Tile(Colors.DeepSkyBlue), null, null},
                        {null, new Tile(Colors.DeepSkyBlue), null, null},
                        {null, new Tile(Colors.DeepSkyBlue), null, null}
                    };
                    break;

                //todo add more figures
            }

            return figure;
        }
    }
}