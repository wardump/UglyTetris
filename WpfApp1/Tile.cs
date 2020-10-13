using System.Windows.Media;

namespace WpfApp1
{
    public class Tile
    {
        public Tile(Color color)
        {
            Color = color;
        }

        public Color Color { get; private set; }
    }
}