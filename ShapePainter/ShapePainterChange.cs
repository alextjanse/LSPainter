using LSPainter.Maths;
using LSPainter.Maths.Shapes;
using LSPainter.LSSolver.Painter;

namespace LSPainter.ShapePainter
{
    public class ShapePainterChange : CanvasChange
    {
        public Shape Shape { get; }
        public Color Color { get; }

        public ShapePainterChange(Shape shape, Color color)
        {
            Shape = shape;
            Color = color;
        }
    }
}