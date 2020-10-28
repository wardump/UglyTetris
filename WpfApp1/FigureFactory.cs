using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace WpfApp1
{
    public enum FigureType
    {
        I,O,T,S,Z,J,L
    }

    public class FigureFactory
    {
        public Figure CreateStandardFigure(FigureType figureType)
        {
            string tiles = "";
            var nl = Environment.NewLine;
            Color color;


            switch (figureType)
            {
                case FigureType.I:
                    tiles = "  x " + nl +
                            "  x " + nl +
                            "  x " + nl +
                            "  x ";
                    color = Colors.Cyan;
                    break;

                case FigureType.O:
                    tiles = "    " + nl +
                            " xx " + nl +
                            " xx " + nl +
                            "    ";
                    color = Colors.Yellow;
                    break;

                case FigureType.T:
                    tiles = "    " + nl +
                            " xxx" + nl +
                            "  x " + nl +
                            "    ";
                    color = Colors.BlueViolet;
                    break;

                case FigureType.S:
                    tiles = " x  " + nl +
                            " xx " + nl +
                            "  x " + nl +
                            "    ";
                    color = Colors.Red;
                    break;

                case FigureType.Z:
                    tiles = "  x " + nl +
                            " xx " + nl +
                            " x  " + nl +
                            "    ";
                    color = Colors.Brown;
                    break;

                case FigureType.J:
                    tiles = "  x " + nl +
                            "  x " + nl +
                            " xx " + nl +
                            "    ";
                    color = Colors.Blue;
                    break;

                case FigureType.L:
                    tiles = " x  " + nl +
                            " x  " + nl +
                            " xx " + nl +
                            "    ";
                    color = Colors.Green;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return new Figure(tiles, color);
        }

        public Figure CreateRandomFigure()
        {
            var r = new Random();
            var randomType = r.Next(0, 7); // code smell

            return CreateStandardFigure((FigureType) randomType);
        }
    }
}
