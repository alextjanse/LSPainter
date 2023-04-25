using LSPainter.Maths;
using LSPainter.Maths.Shapes;
using LSPainter.Solver;

namespace LSPainter.ShapePainter
{
    public class ShapeChange : IChange
    {
        public Shape Shape { get; }
        public Color Color { get; }

        public ShapeChange(Shape shape, Color color)
        {
            Shape = shape;
            Color = color;
        }

        public void ApplyToCanvas(LSPainter.Painting canvas)
        {
            canvas.DrawShape(Shape, Color);
        }

        public BoundingBox GenerateBoundingBox()
        {
            return Shape.CreateBoundingBox();
        }
    }
}