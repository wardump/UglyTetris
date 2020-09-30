using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;
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

        public int Width => Tiles.GetUpperBound(0) + 1;
        public int Height => Tiles.GetUpperBound(1) + 1;

        public void Draw(double left, double top)
        {
            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    if (Tiles[i, j] != null)
                    {
                        Canvas.SetLeft(Tiles[i, j], left + i * FieldHelper.BlockWidth + 1);
                        Canvas.SetTop(Tiles[i, j], top + j * FieldHelper.BlockHeight + 1);
                    }
                }
            }
        }

        public void RotateLeft()
        {
            var newTiles = new Rectangle[Height, Width]; // rotation swaps coordinates

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
            var newTiles = new Rectangle[Height, Width]; // rotation swaps coordinates

            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    newTiles[Height - j - 1, i] = Tiles[i, j];
                }
            }

            Tiles = newTiles;
        }

        public static Figure CreateRandomFigure(Func<Color, Rectangle> newRectangle)
        {
            var r = new Random();
            var figure = new Figure();
            switch (r.Next(1, 5))
            {
                case 1:
                    figure.Tiles = new Rectangle[,]
                    {
                        {null, newRectangle(Colors.Red), newRectangle(Colors.Red), null},
                        {null, null, newRectangle(Colors.Red), null},
                        {null, null, newRectangle(Colors.Red), null},
                        {null, null, null, null}
                    };
                    break;
                case 2:
                    figure.Tiles = new Rectangle[,]
                    {
                        {null, newRectangle(Colors.LawnGreen), newRectangle(Colors.LawnGreen), null},
                        {null, newRectangle(Colors.LawnGreen), null, null},
                        {null, newRectangle(Colors.LawnGreen), null, null},
                        {null, null, null, null}
                    };
                    break;
                case 3:
                    figure.Tiles = new Rectangle[,]
                    {
                        {null, null, null, null},
                        {null, newRectangle(Colors.Brown), newRectangle(Colors.Brown), null},
                        {null, newRectangle(Colors.Brown), newRectangle(Colors.Brown), null},
                        {null, null, null, null}
                    };
                    break;
                case 4:
                    figure.Tiles = new Rectangle[,]
                    {
                        {null, newRectangle(Colors.DeepSkyBlue), null, null},
                        {null, newRectangle(Colors.DeepSkyBlue), null, null},
                        {null, newRectangle(Colors.DeepSkyBlue), null, null},
                        {null, newRectangle(Colors.DeepSkyBlue), null, null}
                    };
                    break;

                //todo add more figures
            }

            return figure;
        }
    }
}