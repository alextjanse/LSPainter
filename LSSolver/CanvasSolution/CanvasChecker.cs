using LSPainter.Maths;

namespace LSPainter.LSSolver.CanvasSolution
{
    public class CanvasChecker : IChecker<CanvasSolution, CanvasChange>
    {
        private ImageHandler original { get; }

        public CanvasChecker(ImageHandler original)
        {
            this.original = original;
        }

        public long ScoreSolution(CanvasSolution solution)
        {
            long score = 0;

            for (int y = 0; y < solution.Canvas.Height; y++)
            {
                for (int x = 0; x < solution.Canvas.Width; x++)
                {
                    score += PixelScoreDiff(x, y, solution.Canvas.GetPixel(x, y));
                }
            }

            return score;
        }

        public long PixelScoreDiff(int x, int y, Color color)
        {
            Color originalColor = original.GetPixel(x, y);

            (int r, int g, int b) = Color.Diff(originalColor, color);

            return r * r + g * g + b * b;
        }

        public long ScoreChange(CanvasSolution solution, CanvasChange change)
        {
            long scoreDiff = 0;

            BoundingBox bbox = change.BoundingBox;

            foreach ((int x, int y) in bbox.AsEnumerable())
            {
                Color currentColor = solution.Canvas.GetPixel(x, y);
                Color newColor = change.GetPixel(x, y);

                long currentScore = PixelScoreDiff(x, y, currentColor);
                long newScore = PixelScoreDiff(x, y, newColor);

                scoreDiff += newScore - currentScore;
            }

            return scoreDiff;
        }
    }
}