using LSPainter.Maths;

namespace LSPainter.FiniteShapePainter.Operations
{
    public class InsertOperation : FiniteShapePainterOperation
    {
        public (Shape, Color) Obj { get; }
        public int Index { get; }

        public InsertOperation(Shape shape, Color color, int index) : base(shape.BoundingBox)
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

            for (int i = Index; i < solution.NumberOfShapes; i++)
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
            solution.InsertShape(Obj, Index);

            solution.DrawSection(BoudningBox);
        }
    }
}