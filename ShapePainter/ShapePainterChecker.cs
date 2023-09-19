using LSPainter.LSSolver.Painter;

namespace LSPainter.ShapePainter
{
    public class ShapePainterChecker : CanvasChecker<ShapePainterSolution, ShapePainterScore>
    {
        public ShapePainterChecker(ImageHandler originalImage) : base(originalImage)
        {
        }

        public override ShapePainterScore ScoreSolution(ShapePainterSolution solution)
        {
            long totalPixelScore = 0;
            long blankPixels = 0;

            for (int y = 0; y < solution.Canvas.Height; y++)
            {
                for (int x = 0; x < solution.Canvas.Width; x++)
                {
                    totalPixelScore += GetPixelScore(x, y, solution.Canvas.GetPixel(x, y));
                    blankPixels += PixelIsBlank(solution, x, y) ? 1 : 0;
                }
            }

            return new ShapePainterScore(totalPixelScore, blankPixels);
        }
    }
}