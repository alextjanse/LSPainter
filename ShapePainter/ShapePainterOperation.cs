using LSPainter.Maths;
using LSPainter.LSSolver;
using LSPainter.Maths.Shapes;
using LSPainter.LSSolver.Painter;

namespace LSPainter.ShapePainter
{
    public abstract class ShapePainterOperation : CanvasOperation<ShapePainterSolution, ShapePainterScore, ShapePainterChecker>
    {
        protected ShapePainterOperation(BoundingBox bbox) : base(bbox)
        {
        }
    }

    public class PaintShapeOperation : ShapePainterOperation
    {
        public Shape Shape { get; }
        public Color Color { get; }

        public PaintShapeOperation(Shape shape, Color color) :base(shape.CreateBoundingBox())
        {
            Shape = shape;
            Color = color;
        }

        public override ShapePainterScore Try(ShapePainterSolution solution, ShapePainterScore currentScore, ShapePainterChecker checker)
        {
            TrimToCanvas(checker);
            
            ShapePainterScore newScore = (ShapePainterScore)currentScore.Clone();

            foreach ((int x, int y) in BBox.AsEnumerable())
            {
                if (Shape.IsInside(GetPixelVector(x, y)))
                {
                    Color currentColor = solution.Canvas.GetPixel(x, y);
                    Color newColor = Color.Blend(solution.Canvas.GetPixel(x, y), Color);

                    long currentPixelScoreDiff = checker.GetPixelScore(x, y, currentColor);
                    long newPixelScoreDiff = checker.GetPixelScore(x, y, newColor);

                    newScore.SquaredPixelDiff += newPixelScoreDiff - currentPixelScoreDiff;
                }
            }

            return newScore;
        }

        public override void Apply(ShapePainterSolution solution)
        {
            foreach ((int x, int y) in BBox.AsEnumerable<(int, int)>())
            {
                if (Shape.IsInside(GetPixelVector(x, y)))
                {
                    Color newColor = Color.Blend(solution.Canvas.GetPixel(x, y), Color);
                    solution.Canvas.SetPixel(x, y, newColor);
                }
            }
        }
    }
}