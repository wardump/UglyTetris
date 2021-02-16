﻿using System;
using System.Linq;

namespace UglyTetris.GameLogic
{
    public class Figure
    {
        private Tile[,] _tiles = new Tile[,]
        {
            {null, null, null, null},
            {null, null, null, null},
            {null, null, null, null},
            {null, null, null, null}
        };

        public Tile[,] GetTiles()
        {
            return _tiles;
        }

        public int Width => _tiles.GetUpperBound(0) + 1;
        public int Height => _tiles.GetUpperBound(1) + 1;
        
        public Figure()
        {
            
        }

        public Figure(Figure figure)
        {
            _tiles = new Tile[figure.Width, figure.Height];

            for (var x = 0; x < figure.Width; x++)
            for (var y = 0; y < figure.Height; y++)
            {
                var tile = figure._tiles[x, y];
                _tiles[x, y] = tile == null ? null : new Tile(tile.Color);
            }
        }

        public Figure(string tileMap, string color)
        {
            var lines = tileMap.Split(Environment.NewLine);

            var width = lines.Max(l => l.Length);
            var height = lines.Length;

            _tiles = new Tile[width, height];

            for (var y = 0; y < height; y++)
            {
                var line = lines[y];
                for (var x = 0; x < width; x++)
                {
                    var c = (x < line.Length) ? line[x] : ' ';

                    var tile = char.IsWhiteSpace(c) ? null : new Tile(color);

                    _tiles[x, y] = tile;
                }
            }
        }

        public void RotateLeft()
        {
            var newTiles = new Tile[Height, Width]; // rotation swaps coordinates

            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    newTiles[j, Width - i - 1] = _tiles[i, j];
                }
            }

            _tiles = newTiles;
        }

        public void RotateRight()
        {
            var newTiles = new Tile[Height, Width]; // rotation swaps coordinates

            for (var i = 0; i < Width; i++)
            {
                for (var j = 0; j < Height; j++)
                {
                    newTiles[Height - j - 1, i] = _tiles[i, j];
                }
            }

            _tiles = newTiles;
        }
    }
}