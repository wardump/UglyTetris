using System;
using FluentAssertions;
using WpfApp1;
using Xunit;

namespace Tests
{
    public class FigureTest
    {
        [Fact]
        public void RotateRightLeftShouldNotChange()
        {
            // this test is bad because it is not consistent
            // It relies on random figure
            // We should create a set of non-random figures, but current Figure class does not allow
            // TODO refactor code and extract figure factory
            var figure = Figure.CreateRandomFigure();

            var newFigure = CopyFigure(figure);

            newFigure.Should().BeEquivalentTo(figure);

            newFigure.RotateLeft();
            newFigure.RotateRight();
            
            newFigure.Should().BeEquivalentTo(figure);

            newFigure.RotateRight();
            newFigure.RotateLeft();

            newFigure.Should().BeEquivalentTo(figure);
        }


        [Fact]
        public void RotateRight4TimesShouldNotChange()
        {
            // this test is bad because it is not consistent
            // It relies on random figure
            // We should create a set of non-random figures, but current Figure class does not allow
            // TODO refactor code and extract figure factory
            var figure = Figure.CreateRandomFigure();

            var newFigure = CopyFigure(figure);

            newFigure.Should().BeEquivalentTo(figure);
            
            newFigure.RotateRight();
            newFigure.RotateRight();
            newFigure.RotateRight();
            newFigure.RotateRight();
            
            newFigure.Should().BeEquivalentTo(figure);
        }

        [Fact]
        public void RotateLeft4TimesShouldNotChange()
        {
            // this test is bad because it is not consistent
            // It relies on random figure
            // We should create a set of non-random figures, but current Figure class does not allow
            // TODO refactor code and extract figure factory
            var figure = Figure.CreateRandomFigure();

            var newFigure = CopyFigure(figure);

            newFigure.Should().BeEquivalentTo(figure);

            newFigure.RotateLeft();
            newFigure.RotateLeft();
            newFigure.RotateLeft();
            newFigure.RotateLeft();

            newFigure.Should().BeEquivalentTo(figure);
        }

        private Figure CopyFigure(Figure figure)
        {
            var newFigure = new Figure();

            newFigure.Tiles = new Tile[figure.Width, figure.Height];

            for (var x = 0; x < figure.Width; x++)
            for (var y = 0; y < figure.Height; y++)
            {
                var tile = figure.Tiles[x, y];
                newFigure.Tiles[x, y] = tile == null ? null : new Tile(tile.Color);
            }

            return newFigure;
        }
    }
}
