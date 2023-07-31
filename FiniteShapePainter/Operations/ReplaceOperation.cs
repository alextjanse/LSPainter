using LSPainter.Maths;

namespace LSPainter.FiniteShapePainter.Operations
{
    public class ReplaceOperation : FiniteShapePainterOperation
    {
        public (Shape, Color) Obj { get; }
        public Color Color { get; }
        public int Index { get; }

        public ReplaceOperation(Shape shape, Color color, Rectangle boundingBox, int index) : base(boundingBox)
        {
            Obj = (shape, color);
            Index = index;
        }

        public override FiniteShapePainterScore Try(FiniteShapePainterSolution solution, FiniteShapePainterScore currentScore, FiniteShapePainterChecker checker)
        {
            TrimToCanvas(checker);

            int minX = (int)BoudningBox.MinX;
            int minY = (int)BoudningBox.MinY;

            Color[,] section = new Color[BoudningBox.SectionWidth, BoudningBox.SectionHeight];

            for (int i = 0; i < Index; i++)
            {
                (Shape, Color) obj = solution.Shapes[i];

                DrawShapeOnSection(ref section, obj);
            }

            DrawShapeOnSection(ref section, Obj);

            for (int i = Index + 1; i < solution.NumberOfShapes; i++)
            {
                (Shape, Color) obj = solution.Shapes[i];

                DrawShapeOnSection(ref section, obj);
            }

            long pixelScoreDiff = GetSectionScoreDiff(section, minX, minY, solution, checker);

            FiniteShapePainterScore newScore = (FiniteShapePainterScore)currentScore.Clone();

            newScore.NumberOfShapes++;
            newScore.SquaredPixelDiff += pixelScoreDiff;

            return newScore;
        }

        public override void Apply(FiniteShapePainterSolution solution)
        {
            solution.RemoveAt(Index);
            solution.InsertShape(Obj, Index);

            solution.DrawSection(BoudningBox);
        }
    }
}