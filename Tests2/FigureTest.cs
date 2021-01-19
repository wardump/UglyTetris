using FluentAssertions;
using UglyTetris.GameLogic;
using Xunit;

namespace Tests2
{
    public class FigureTest
    {
        [Theory]
        [InlineData(FigureType.I)]
        [InlineData(FigureType.J)]
        public void RotateRightLeftShouldNotChange(FigureType figureType)
        {
            var figureFactory = new FigureFactory();

            var figure = figureFactory.CreateStandardFigure(figureType);

            var newFigure = new Figure(figure);

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
            
            var figureFactory = new FigureFactory();
            var figure = figureFactory.CreateRandomFigure();

            var newFigure = new Figure(figure);

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

            var figureFactory = new FigureFactory();
            var figure = figureFactory.CreateRandomFigure();

            var newFigure = new Figure(figure);

            newFigure.Should().BeEquivalentTo(figure);

            newFigure.RotateLeft();
            newFigure.RotateLeft();
            newFigure.RotateLeft();
            newFigure.RotateLeft();

            newFigure.Should().BeEquivalentTo(figure);
        }


        [Theory]
        [InlineData("  x \r\n  x \r\n  x \r\n  x ", "    \r\nxxxx\r\n    \r\n    ")]
        public void RotateLeft(string figureTiles, string figureTilesRotated)
        {
            var figure = new Figure(figureTiles, "Yellow");
            var figureRotated = new Figure(figureTilesRotated, "Yellow");

            figure.RotateLeft();

            figure.Should().BeEquivalentTo(figureRotated);
        }

        [Fact]
        public void CreateRandomFigure()
        {
            var figureFactory = new FigureFactory();

            for (var i = 0; i < 100; i++)
            {
                var figure = figureFactory.CreateRandomFigure();
            }
        }
    }
}
