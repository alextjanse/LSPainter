using LSPainter.Maths;

namespace LSPainter.FiniteShapePainter.Operations
{
    public class ReorderOperation : FiniteShapePainterOperation
    {
        public int Index1 { get; }
        public int Index2 { get; }

        public ReorderOperation(int index1, int index2, Rectangle boundingBox) : base(boundingBox)
        {
            Index1 = Math.Min(index1, index2);
            Index2 = Math.Max(index1, index2);
        }

        public override FiniteShapePainterScore Try(FiniteShapePainterSolution solution, FiniteShapePainterScore currentScore, FiniteShapePainterChecker checker)
        {
            (Shape shape1, Color color1) = solution.Shapes[Index1];
            (Shape shape2, Color color2) = solution.Shapes[Index2];

            for (int i = 0; i < Index1; i++)
            {
                (Shape shape, Color color) = solution.Shapes[i];

                Sketch.DrawShape(shape, color);
            }

            Sketch.DrawShape(shape2, color2);

            for (int i = Index1 + 1; i < Index2; i++)
            {
                (Shape shape, Color color) = solution.Shapes[i];

                Sketch.DrawShape(shape, color);
            }

            Sketch.DrawShape(shape1, color1);

            for (int i = Index2 + 1; i < solution.NumberOfShapes; i++)
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
            var o1 = solution.Shapes[Index1];
            var o2 = solution.Shapes[Index2];

            solution.Shapes[Index1] = o2;
            solution.Shapes[Index2] = o1;

            solution.DrawSection(BoundingBox);
        }
    }
}