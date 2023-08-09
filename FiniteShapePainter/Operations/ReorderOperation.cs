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
            TrimToCanvas(checker);

            int minX = (int)BoudningBox.MinX;
            int minY = (int)BoudningBox.MinY;

            Color[,] section = new Color[BoudningBox.SectionWidth, BoudningBox.SectionHeight];

            (Shape, Color) obj1 = solution.Shapes[Index1];
            (Shape, Color) obj2 = solution.Shapes[Index2];

            for (int i = 0; i < Index1; i++)
            {
                (Shape, Color) obj = solution.Shapes[i];

                DrawShapeOnSection(ref section, obj);
            }

            DrawShapeOnSection(ref section, obj2);

            for (int i = Index1 + 1; i < Index2; i++)
            {
                (Shape, Color) obj = solution.Shapes[i];

                DrawShapeOnSection(ref section, obj);
            }

            DrawShapeOnSection(ref section, obj1);

            for (int i = Index2 + 1; i < solution.NumberOfShapes; i++)
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
            var o1 = solution.Shapes[Index1];
            var o2 = solution.Shapes[Index2];

            solution.Shapes[Index1] = o2;
            solution.Shapes[Index2] = o1;

            solution.DrawSection(BoudningBox);
        }
    }
}