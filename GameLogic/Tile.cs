namespace UglyTetris.GameLogic
{
    public class Tile
    {
        public Tile(string color)
        {
            Color = color;
        }

        public string Color { get; private set; } // there is no cross-platform color type in .NET
    }
}