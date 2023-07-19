namespace LSPainter.LSSolver.Painter
{
    public abstract class CanvasChecker<TSolution, TScore> : SolutionChecker<TSolution, TScore> where TSolution : CanvasSolution where TScore : CanvasScore<TSolution>
    {
        public ImageHandler OriginalImage { get; }

        public CanvasChecker(ImageHandler originalImage)
        {
            OriginalImage = originalImage;
        }

        public long GetPixelScore(int x, int y, Color color)
        {
            (int r, int g, int b) = Color.Diff(OriginalImage.GetPixel(x, y), color);

            return r * r + g * g + b * b;
        }
    }
}