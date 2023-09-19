using LSPainter.Maths;

namespace LSPainter.FiniteShapePainter.Operations
{
    public class TranslateOperation : FiniteShapePainterOperation
    {
        public Vector Translation { get; }

        public TranslateOperation(int index, Vector translation, Rectangle boundingBox) : base(index, boundingBox)
        {
            Translation = translation;
        }

        public override FiniteShapePainterScore Try(FiniteShapePainterSolution solution, FiniteShapePainterScore currentScore, FiniteShapePainterChecker checker)
        {
            for (int i = 0; i < Index; i++)
            {
                (Shape shape, Color color) = solution.Shapes[i];

                Sketch.DrawShape(shape, color);
            }

            (Shape s, Color c) = solution.Shapes[Index];

            Shape translated = (Shape)s.Clone();

            translated.Translate(Translation);

            Sketch.DrawShape(translated, c);

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

            s.Translate(Translation);

            if (!s.BoundingBox.Overlaps(solution.Canvas.BoundingBox))
            {
                solution.RemoveAt(Index);
            }

            solution.DrawSection(BoundingBox);
        }
    }
}