using UglyTetris.GameLogic;

namespace UglyTetris.AvaloniaGUI
{
    public interface IDrawFigure
    {
        void DrawFigure(Figure figure, int x, int y);
    }
}