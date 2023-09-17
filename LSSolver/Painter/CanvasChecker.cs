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

        public long GetPixelScore(TSolution solution, Rectangle boundingBox)
        {
            long pixelScore = 0;

            foreach ((int x, int y) in boundingBox.PixelCoords())
            {
                pixelScore += GetPixelScore(x, y, solution.Canvas.GetPixel(x, y));
            }

            return pixelScore;
        }

        public long GetBlankPixelCount(TSolution solution)
        {
            long result = 0;

            foreach ((int x, int y) in solution.Canvas.BBox.PixelCoords())
            {
                if (PixelIsBlank(solution, x, y)) result++;
            }

            return result;
        }

        public long GetBlankPixelCount(TSolution solution, Rectangle boundingBox)
        {
            long result = 0;

            foreach ((int x, int y) in boundingBox.PixelCoords())
            {
                if (PixelIsBlank(solution, x, y)) result++;
            }

            return result;
        }

        public bool PixelIsBlank(TSolution solution, int x, int y)
        {
            return solution.Canvas.GetPixel(x, y).A == 0;
        }

        public (long PixelScore, long BlankPixels) ScoreCanvasSketch(CanvasSketch sketch)
        {
            long pixelScore = 0;
            long blankPixels = 0;

            for (int y = 0; y < sketch.Height; y++)
            {
                for (int x = 0; x < sketch.Width; x++)
                {
                    int xCanvas = sketch.OriginOffsets.x + x;
                    int yCanvas = sketch.OriginOffsets.y + y;

                    pixelScore += GetPixelScore(xCanvas, yCanvas, sketch.Colors[x, y]);

                    if (sketch.Colors[x, y] == Color.None) blankPixels++;
                }
            }

            return (pixelScore, blankPixels);
        }
    }
}