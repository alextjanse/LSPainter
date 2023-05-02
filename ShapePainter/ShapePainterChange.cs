using LSPainter.Maths;
using LSPainter.Maths.Shapes;
using LSPainter.LSSolver.CanvasSolution;

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

        public void ApplyToCanvas(Painting canvas)
        {
            canvas.DrawShape(Shape, Color);
        }
    }
}