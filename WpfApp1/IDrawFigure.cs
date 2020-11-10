using UglyTetris.GameLogic;

namespace WpfApp1
{
    public interface IDrawFigure
    {
        void DrawFigure(Figure figure, int x, int y);
    }
}