using LSPainter.Maths;

namespace LSPainter.LSSolver.Painter
{
    public abstract class CanvasChecker<TSolution, TScore> : SolutionChecker<TSolution, TScore> where TSolution : CanvasSolution where TScore : CanvasScore<TSolution>
    {
        public ImageHandler OriginalImage { get; }

        public CanvasChecker(ImageHandler originalImage)
        {
            OriginalImage = originalImage;
        }

        public long GetTotalPixelScore(TSolution solution)
        {
            long totalPixelScore = 0;

            foreach ((int x, int y) in solution.Canvas.BBox.PixelCoords())
            {
                totalPixelScore += GetPixelScore(x, y, solution.Canvas.GetPixel(x, y));
            }

            return totalPixelScore;
        }

        public long GetPixelScore(int x, int y, Color color)
        {
            (int r, int g, int b) = Color.Diff(OriginalImage.GetPixel(x, y), color);

            return r * r + g * g + b * b;
        }
    }
}