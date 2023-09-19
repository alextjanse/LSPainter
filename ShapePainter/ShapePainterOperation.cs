using LSPainter.Maths;
using LSPainter.LSSolver;
using LSPainter.LSSolver.Painter;

namespace LSPainter.ShapePainter
{
    public abstract class ShapePainterOperation : CanvasOperation<ShapePainterSolution, ShapePainterScore, ShapePainterChecker>
    {
        protected ShapePainterOperation(Rectangle bbox) : base(bbox)
        {
        }
    }

    public class PaintShapeOperation : ShapePainterOperation
    {
        public Shape Shape { get; }
        public Color Color { get; }

        public PaintShapeOperation(Shape shape, Color color) :base(shape.BoundingBox)
        {
            Shape = shape;
            Color = color;
        }

        public override ShapePainterScore Try(ShapePainterSolution solution, ShapePainterScore currentScore, ShapePainterChecker checker)
        {
            TrimToCanvas(checker);
            
            ShapePainterScore newScore = (ShapePainterScore)currentScore.Clone();

            long blankPixelDiff = 0;

            foreach ((int x, int y) in BoundingBox.PixelCoords())
            {
                if (Shape.IsInside(GetPixelVector(x, y)))
                {
                    Color currentColor = solution.Canvas.GetPixel(x, y);
                    
                    if (checker.PixelIsBlank(solution, x, y)) blankPixelDiff--;

                    Color newColor = Color.Blend(currentColor, Color);

                    long currentPixelScoreDiff = checker.GetPixelScore(x, y, currentColor);
                    long newPixelScoreDiff = checker.GetPixelScore(x, y, newColor);

                    newScore.SquaredPixelDiff += newPixelScoreDiff - currentPixelScoreDiff;
                }
            }

            newScore.BlankPixels += blankPixelDiff;

            return newScore;
        }

        public override void Apply(ShapePainterSolution solution)
        {
            foreach ((int x, int y) in BoundingBox.PixelCoords())
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