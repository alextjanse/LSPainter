using LSPainter.Maths;

namespace LSPainter.FiniteShapePainter.Operations
{
    public class InsertOperation : FiniteShapePainterOperation
    {
        public Shape Shape { get; }
        public Color Color { get; }
        public int Index { get; }

        public InsertOperation(Shape shape, Color color, int index) : base(shape.BoundingBox)
        {
            Shape = shape;
            Color = color;
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
                (Shape shape, Color color) = solution.Shapes[i];

                DrawShapeOnSection(ref section, shape, color);
            }

            DrawShapeOnSection(ref section, Shape, Color);

            for (int i = Index; i < solution.Shapes.Count; i++)
            {
                (Shape shape, Color color) = solution.Shapes[i];

                DrawShapeOnSection(ref section, shape, color);
            }

            long pixelScoreDiff = GetSectionScoreDiff(section, minX, minY, solution, checker);

            FiniteShapePainterScore newScore = (FiniteShapePainterScore)currentScore.Clone();

            newScore.NumberOfShapes++;
            newScore.SquaredPixelDiff += pixelScoreDiff;

            return newScore;
        }

        public override void Apply(FiniteShapePainterSolution solution)
        {
            solution.InsertShape(Shape, Color, Index);

            solution.DrawSection(BoudningBox);
        }
    }
}