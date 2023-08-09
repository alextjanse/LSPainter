using LSPainter.LSSolver.Painter;
using LSPainter.Maths;

namespace LSPainter.FiniteShapePainter
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

        protected void DrawShapeOnSection(ref Color[,] section, (Shape, Color) obj)
        {
            (Shape shape, Color color) = obj;

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

        protected (long pixelScoreDiff, long blankPixelDiff) GetSectionScoreDiff(Color[,] section, int minX, int minY, FiniteShapePainterSolution solution, FiniteShapePainterChecker checker)
        {
            long pixelScoreDiff = 0;
            long blankPixelDiff = 0;

            for (int y = 0; y < section.GetLength(1); y++)
            {
                for (int x = 0; x < section.GetLength(0); x++)
                {
                    int xIndex = minX + x;
                    int yIndex = minY + y;

                    pixelScoreDiff += checker.GetPixelScore(xIndex, yIndex, section[x, y])
                                    - checker.GetPixelScore(xIndex, yIndex, solution.Canvas.GetPixel(xIndex, yIndex));

                    blankPixelDiff += checker.PixelIsBlank(solution, xIndex, yIndex) && section[x, y].A > 0 ? -1 : 0;
                    blankPixelDiff += !checker.PixelIsBlank(solution, xIndex, yIndex) && section[x, y].A == 0 ? 1 : 0;
                }
            }

            return (pixelScoreDiff, blankPixelDiff);
        }

        public abstract override void Apply(FiniteShapePainterSolution solution);
    }
}