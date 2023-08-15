using LSPainter.Maths;

namespace LSPainter.FiniteShapePainter.Operations
{
    public class TranslateOperation : FiniteShapePainterOperation
    {
        public Vector Translation { get; }
        public int Index { get; }

        public TranslateOperation(int index, Vector translation, Rectangle boundingBox) : base(boundingBox)
        {
            Index = index;
            Translation = translation;
        }

        public override FiniteShapePainterScore Try(FiniteShapePainterSolution solution, FiniteShapePainterScore currentScore, FiniteShapePainterChecker checker)
        {
            TrimToCanvas(checker);

            int minX = (int)BoundingBox.MinX;
            int minY = (int)BoundingBox.MinY;

            Color[,] section = new Color[BoundingBox.SectionWidth, BoundingBox.SectionHeight];

            for (int i = 0; i < Index; i++)
            {
                (Shape, Color) obj = solution.Shapes[i];

                DrawShapeOnSection(ref section, obj);
            }

            (Shape s, Color c) = solution.Shapes[Index];

            Shape translated = (Shape)s.Clone();

            translated.Translate(Translation);

            DrawShapeOnSection(ref section, (translated, c));

            for (int i = Index + 1; i < solution.NumberOfShapes; i++)
            {
                (Shape, Color) obj = solution.Shapes[i];

                DrawShapeOnSection(ref section, obj);
            }

            (long pixelScoreDiff, long blankPixelDiff) = GetSectionScoreDiff(section, minX, minY, solution, checker);

            FiniteShapePainterScore newScore = (FiniteShapePainterScore)currentScore.Clone();

            newScore.SquaredPixelDiff += pixelScoreDiff;
            newScore.BlankPixels += blankPixelDiff;

            return newScore;
        }

        public override void Apply(FiniteShapePainterSolution solution)
        {
            (Shape s, Color c) = solution.Shapes[Index];

            s.Translate(Translation);

            solution.DrawSection(BoundingBox);
        }
    }
}