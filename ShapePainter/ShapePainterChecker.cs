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
            long pixelScoreDiff = 0;
            
            int r, g, b;
            for (int y = 0; y < solution.Canvas.Height; y++)
            {
                for (int x = 0; x < solution.Canvas.Width; x++)
                {
                    (r, g, b) = Color.Diff(solution.Canvas.GetPixel(x, y), OriginalImage.GetPixel(x, y));

                    pixelScoreDiff += r * r + b * b + g * g;
                }
            }

            return new ShapePainterScore(pixelScoreDiff);
        }
    }
}