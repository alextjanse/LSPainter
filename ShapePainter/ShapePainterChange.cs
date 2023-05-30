using LSPainter.Maths;
using LSPainter.Maths.Shapes;
using LSPainter.LSSolver.Painter;
using LSPainter.LSSolver;

namespace LSPainter.ShapePainter
{
    public class ShapePainterChange : CanvasChange<ShapePainterSolution>
    {
        public override BoundingBox BoundingBox => Shape.CreateBoundingBox();
        public Shape Shape { get; }
        public Color Color { get; }

        public ShapePainterChange(Shape shape, Color color)
        {
            Shape = shape;
            Color = color;
        }

        public override ShapePainterScore Try(ShapePainterSolution solution, ISolutionChecker<CanvasSolution> checker)
        {
            long pixelDiff = TryDrawShape(solution, (CanvasSolutionChecker)checker, Shape, Color);

            return new ShapePainterScore(pixelDiff);
        }

        public override void Apply(ShapePainterSolution solution)
        {
            DrawShape(solution, Shape, Color);
        }
    }
}