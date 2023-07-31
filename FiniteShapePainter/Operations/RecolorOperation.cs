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
            TrimToCanvas(checker);

            int minX = (int)BoudningBox.MinX;
            int minY = (int)BoudningBox.MinY;

            Color[,] section = new Color[BoudningBox.SectionWidth, BoudningBox.SectionHeight];

            for (int i = 0; i < Index; i++)
            {
                (Shape shape, Color color) = solution.Shapes[i];

                DrawShapeOnSection(ref section, shape, color);
            }

            (Shape s, Color c) = solution.Shapes[Index];

            Color newColor = Color.Blend(c, Color);

            DrawShapeOnSection(ref section, s, newColor);

            for (int i = Index + 1; i < solution.Shapes.Count; i++)
            {
                (Shape shape, Color color) = solution.Shapes[i];

                DrawShapeOnSection(ref section, shape, color);
            }

            long pixelScoreDiff = GetSectionScoreDiff(section, minX, minY, solution, checker);

            FiniteShapePainterScore newScore = (FiniteShapePainterScore)currentScore.Clone();

            newScore.SquaredPixelDiff += pixelScoreDiff;

            return newScore;
        }

        public override void Apply(FiniteShapePainterSolution solution)
        {
            (Shape s, Color c) = solution.Shapes[Index];
            
            Color newColor = Color.Blend(c, Color);
            solution.Shapes[Index] = (s, newColor);

            solution.DrawSection(BoudningBox);
        }
    }
}