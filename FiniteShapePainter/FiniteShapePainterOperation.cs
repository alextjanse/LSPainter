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

            Rectangle intersection = Rectangle.Intersect(BoundingBox, shape.BoundingBox);

            if (intersection.IsEmpty) return;

            (int xOffset, int yOffset) = intersection.OriginOffsets;

            foreach ((int xSolution, int ySolution) in intersection.PixelCoords())
            {
                if (shape.IsInside(GetPixelVector(xSolution, ySolution)))
                {
                    int xSection = xSolution - xOffset;
                    int ySection = ySolution - yOffset;
                    section[xSection, ySection] = Color.Blend(section[xSection, ySection], color);
                }
            }
        }

        protected (long pixelScoreDiff, long blankPixelDiff) GetSectionScoreDiff(Color[,] section, int xOffset, int yOffset, FiniteShapePainterSolution solution, FiniteShapePainterChecker checker)
        {
            long pixelScoreDiff = 0;
            long blankPixelDiff = 0;

            int sectionWidth = section.GetLength(0);
            int sectionHeight = section.GetLength(1);

            for (int y = 0; y < sectionHeight; y++)
            {
                for (int x = 0; x < sectionWidth; x++)
                {
                    int xSolution = xOffset + x;
                    int ySolution = yOffset + y;

                    Color currentColor = solution.Canvas.GetPixel(xSolution, ySolution);
                    Color newColor = section[x, y];

                    bool pixelIsBlank = checker.PixelIsBlank(solution, x, y);
                    bool pixelWillBeBlank = newColor == Color.None;

                    if (pixelIsBlank && pixelWillBeBlank) continue;

                    if (pixelIsBlank && !pixelWillBeBlank) blankPixelDiff--;
                    if (!pixelIsBlank && pixelWillBeBlank) blankPixelDiff++;

                    long currentPixelScore = checker.GetPixelScore(xSolution, ySolution, currentColor);
                    long newPixelScore = checker.GetPixelScore(xSolution, ySolution, newColor);

                    pixelScoreDiff += newPixelScore - currentPixelScore;
                }
            }

            return (pixelScoreDiff, blankPixelDiff);
        }

        public abstract override void Apply(FiniteShapePainterSolution solution);
    }
}