using LSPainter.LSSolver.Painter;
using LSPainter.Maths;
using LSPainter.Maths.Shapes;

namespace LSPainter.ShapePainter.FiniteShapePainter
{
    public struct TryingCanvas
    {
        public (int, int) CanvasCoordinates;
        public Color[,] Canvas;
    }
    
    public abstract class FiniteShapePainterOperation : CanvasOperation<FiniteShapePainterSolution, FiniteShapePainterScore, FiniteShapePainterChecker>
    {
        protected FiniteShapePainterOperation(Rectangle bbox) : base(bbox)
        {
        }

        public abstract override FiniteShapePainterScore Try(FiniteShapePainterSolution solution, FiniteShapePainterScore currentScore, FiniteShapePainterChecker checker);

        protected void DrawShapeOnSection(ref Color[,] section, Shape shape, Color color)
        {
            Rectangle intersection = Rectangle.Intersect(BoudningBox, shape.BoundingBox);

            if (intersection.IsEmpty) return;

            (int xOffset, int yOffset) = intersection.OriginOffsets;

            foreach ((int x, int y) in intersection.PixelCoords())
            {
                if (shape.IsInside(GetPixelVector(x, y)))
                {
                    section[x - xOffset, y - yOffset] = Color.Blend(section[x - xOffset, y - yOffset], color);
                }
            }
        }

        protected long GetSectionScoreDiff(Color[,] section, int minX, int minY, FiniteShapePainterSolution solution, FiniteShapePainterChecker checker)
        {
            long pixelScoreDiff = 0;

            for (int y = 0; y < section.GetLength(1); y++)
            {
                for (int x = 0; x < section.GetLength(0); x++)
                {
                    int xIndex = minX + x;
                    int yIndex = minY + y;

                    pixelScoreDiff += checker.GetPixelScore(xIndex, yIndex, solution.Canvas.GetPixel(xIndex, yIndex))
                                    - checker.GetPixelScore(xIndex, yIndex, section[x, y]);
                }
            }

            return pixelScoreDiff;
        }

        public abstract override void Apply(FiniteShapePainterSolution solution);
    }

    public class InsertNewShapeOperation : FiniteShapePainterOperation
    {
        public Shape Shape { get; }
        public Color Color { get; }
        public int Index { get; }

        public InsertNewShapeOperation(Shape shape, Color color, int index) : base(shape.BoundingBox)
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