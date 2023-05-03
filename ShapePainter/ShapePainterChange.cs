using LSPainter.Maths;
using LSPainter.Maths.Shapes;
using LSPainter.LSSolver.Canvas;

namespace LSPainter.ShapePainter
{
    public class ShapePainterChange : CanvasChange
    {
        public Shape Shape { get; }
        public Color Color { get; }
        public override BoundingBox BoundingBox => Shape.CreateBoundingBox();

        public ShapePainterChange(Shape shape, Color color)
        {
            Shape = shape;
            Color = color;
        }

        public override Color GetPixel(int x, int y, Color currentColor)
        {
            return Shape.IsInside(x, y) ? Color.Blend(currentColor, Color) : currentColor;
        }
    }
}