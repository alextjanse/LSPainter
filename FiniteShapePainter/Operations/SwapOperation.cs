using LSPainter.Maths;

namespace LSPainter.FiniteShapePainter.Operations
{
    public class SwapOperation : FiniteShapePainterOperation
    {
        public int SwapIndex { get; }

        public SwapOperation(int index, int swapIndex, Rectangle boundingBox) : base(index, boundingBox)
        {
            SwapIndex = swapIndex;

            if (index >= swapIndex) throw new Exception();
        }

        public override FiniteShapePainterScore Try(FiniteShapePainterSolution solution, FiniteShapePainterScore currentScore, FiniteShapePainterChecker checker)
        {
            (Shape currentShape, Color currentColor) = solution.Shapes[Index];
            (Shape swapShape, Color swapColor) = solution.Shapes[SwapIndex];

            int i;

            for (i = 0; i < Index; i++)
            {
                (Shape shape, Color color) = solution.Shapes[i];

                Sketch.DrawShape(shape, color);
            }

            Sketch.DrawShape(swapShape, swapColor);

            for (i++; i < SwapIndex; i++)
            {
                (Shape shape, Color color) = solution.Shapes[i];

                Sketch.DrawShape(shape, color);
            }

            Sketch.DrawShape(currentShape, currentColor);

            for (i++; i < solution.NumberOfShapes; i++)
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
            var currentShape = solution.Shapes[Index];
            var swapShape = solution.Shapes[SwapIndex];

            solution.Shapes[Index] = swapShape;
            solution.Shapes[SwapIndex] = currentShape;

            solution.DrawSection(BoundingBox);
        }
    }
}