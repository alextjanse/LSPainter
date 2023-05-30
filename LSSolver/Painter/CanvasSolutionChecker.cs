namespace LSPainter.LSSolver.Painter
{
    public class CanvasSolutionChecker : ISolutionChecker<CanvasSolution>
    {
        ImageHandler original;

        public CanvasSolutionChecker(ImageHandler original)
        {
            this.original = original;
        }

        public virtual IScore<CanvasSolution> CalculateScore(CanvasSolution solution)
        {
            Canvas canvas = solution.Canvas;

            long totalPixelScoreDiff = 0;

            for (int y = 0; y < canvas.Height; y++)
            {
                for (int x = 0; x < canvas.Width; x++)
                {
                    Color originalColor = original.GetPixel(x, y);
                    Color solutionColor = canvas.GetPixel(x, y);

                    (int rDiff, int gDiff, int bDiff) = Color.Diff(originalColor, solutionColor);

                    totalPixelScoreDiff += rDiff * rDiff + gDiff * gDiff + bDiff * bDiff;
                }
            }

            return new CanvasScore(totalPixelScoreDiff);
        }
        
        public (int, int, int) GetPixelDiff(int x, int y, Color currentColor, Color newColor)
        {
            Color originalColor = original.GetPixel(x, y);

            (int currR, int currG, int currB) = Color.Diff(originalColor, currentColor);
            (int newR, int newG, int newB) = Color.Diff(originalColor, newColor);
            int rDiff = newR - currR,
                gDiff = newG - currG,
                bDiff = newB - currB;
        }
    }
}