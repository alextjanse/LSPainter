using LSPainter.Maths;

namespace LSPainter.FiniteShapePainter.Operations
{
    public class ReplaceOperation : FiniteShapePainterOperation
    {
        public Shape Shape { get; }
        public Color Color { get; }

        public ReplaceOperation(Shape shape, Color color, int index, Rectangle boundingBox) : base(index, boundingBox)
        {
            Shape = shape;
            Color = color;
        }

        public override FiniteShapePainterScore Try(FiniteShapePainterSolution solution, FiniteShapePainterScore currentScore, FiniteShapePainterChecker checker)
        {
            for (int i = 0; i < Index; i++)
            {
                (Shape shape, Color color) = solution.Shapes[i];

                Sketch.DrawShape(shape, color);
            }

            Sketch.DrawShape(Shape, Color);

            for (int i = Index + 1; i < solution.NumberOfShapes; i++)
            {
                (Shape shape, Color color) = solution.Shapes[i];

                Sketch.DrawShape(shape, color);
            }

            FiniteShapePainterScore newScore = (FiniteShapePainterScore)currentScore.Clone();

            (long newPixelScore, long newBlankPixelCount) = checker.ScoreCanvasSketch(Sketch);
            
            long currentPixelScore = checker.GetPixelScore(solution, Sketch.BoundingBox);
            long currentBlankPixelCount = checker.GetBlankPixelCount(solution, Sketch.BoundingBox);

            newScore.SquaredPixelDiff += newPixelScore - currentPixelScore;
            newScore.BlankPixels += newBlankPixelCount - currentBlankPixelCount;

            return newScore;
        }

        public override void Apply(FiniteShapePainterSolution solution)
        {
            solution.RemoveAt(Index);
            solution.InsertShape((Shape, Color), Index);

            solution.DrawSection(BoundingBox);
        }
    }
}