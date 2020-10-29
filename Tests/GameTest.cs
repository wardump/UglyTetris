using System.Windows.Media;
using FluentAssertions;
using WpfApp1;
using Xunit;

namespace Tests
{
    public class GameTest
    {
        [Fact]
        public void RotateFigure()
        {
            //SUT: 
            // Game:
            //   - field
            //   - figure
            // Game.RotateACW

            var game = new Game();

            game.Field = new Tile[,]
            {
                {new Tile(Colors.Brown), null, null, null, null, new Tile(Colors.Brown), },
                {new Tile(Colors.Brown), null, null, null, null, new Tile(Colors.Brown), },
                {new Tile(Colors.Brown), null, null, null, null, new Tile(Colors.Brown), },
                {new Tile(Colors.Brown), null, null, null, null, new Tile(Colors.Brown), },
                {new Tile(Colors.Brown), null, null, null, null, new Tile(Colors.Brown), },
                {new Tile(Colors.Brown), null, null, null, null, new Tile(Colors.Brown), },
                {new Tile(Colors.Brown), new Tile(Colors.Brown), new Tile(Colors.Brown), new Tile(Colors.Brown), new Tile(Colors.Brown), new Tile(Colors.Brown), },
            };

            var figureFactory = new FigureFactory();

            var figure = figureFactory.CreateStandardFigure(FigureType.I);

            var notRotatedFigure = new Figure(figure);

            game.ResetFigure(figure).Should().BeTrue();

            game.Figure.Should().BeEquivalentTo(notRotatedFigure);

            game.RotateAntiClockWise();

            game.Figure.Should().NotBeEquivalentTo(notRotatedFigure);

            //game.MoveLeft();
            //game.MoveLeft();
            //game.MoveLeft();
            //game.MoveLeft();
            //game.MoveLeft();


        }
    }
}