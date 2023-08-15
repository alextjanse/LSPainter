using LSPainter.Maths;

namespace LSPainter.FiniteShapePainter.Operations
{
    public class ResizeOperation : FiniteShapePainterOperation
    {
        public double Scale { get; }
        public int Index { get; }

        public ResizeOperation(int index, double scale, Rectangle boundingBox) : base(boundingBox)
        {
            Index = index;
            Scale = scale;
        }

        public override FiniteShapePainterScore Try(FiniteShapePainterSolution solution, FiniteShapePainterScore currentScore, FiniteShapePainterChecker checker)
        {
            TrimToCanvas(checker);

            (int xOffset, int yOffset) = BoundingBox.OriginOffsets;

            Color[,] section = new Color[BoundingBox.SectionWidth, BoundingBox.SectionHeight];

            for (int i = 0; i < Index; i++)
            {
                (Shape, Color) obj = solution.Shapes[i];

                DrawShapeOnSection(ref section, obj);
            }

            (Shape s, Color c) = solution.Shapes[Index];

            Shape resized = (Shape)s.Clone();

            resized.Resize(Scale);

            DrawShapeOnSection(ref section, (resized, c));

            for (int i = Index + 1; i < solution.NumberOfShapes; i++)
            {
                (Shape, Color) obj = solution.Shapes[i];

                DrawShapeOnSection(ref section, obj);
            }

            (long pixelScoreDiff, long blankPixelDiff) = GetSectionScoreDiff(section, xOffset, yOffset, solution, checker);

            FiniteShapePainterScore newScore = (FiniteShapePainterScore)currentScore.Clone();

            newScore.SquaredPixelDiff += pixelScoreDiff;
            newScore.BlankPixels += blankPixelDiff;
            
            return newScore;
        }

        public override void Apply(FiniteShapePainterSolution solution)
        {
            (Shape s, Color c) = solution.Shapes[Index];

            s.Resize(Scale);

            solution.DrawSection(BoundingBox);
        }
    }
}