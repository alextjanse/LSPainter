using LSPainter.Maths;

namespace LSPainter.FiniteShapePainter.Operations
{
    public class RecolorOperation : FiniteShapePainterOperation
    {
        public Color Color { get; }
        public int Index { get; }

        public RecolorOperation(int index, Color color, Rectangle boundingBox) : base(boundingBox)
        {
            Index = index;
            Color = color;
        }

        public override FiniteShapePainterScore Try(FiniteShapePainterSolution solution, FiniteShapePainterScore currentScore, FiniteShapePainterChecker checker)
        {
            for (int i = 0; i < Index; i++)
            {
                (Shape shape, Color color) = solution.Shapes[i];

                Sketch.DrawShape(shape, color);
            }

            (Shape s, Color c) = solution.Shapes[Index];

            Color newColor = Color.Blend(c, Color);

            Sketch.DrawShape(s, newColor);

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
            (Shape s, Color c) = solution.Shapes[Index];
            
            Color newColor = Color.Blend(c, Color);
            solution.Shapes[Index] = (s, newColor);

            solution.DrawSection(BoundingBox);
        }
    }
}