namespace LSPainter.LSSolver.Painter
{
    public class CanvasComparer
    {
        ImageHandler original;

        public CanvasComparer(ImageHandler original)
        {
            this.original = original;
        }
        
        public long PixelScoreDiff(int x, int y, Color currentColor, Color newColor)
        {
            Color originalColor = original.GetPixel(x, y);

            (int currR, int currG, int currB) = Color.Diff(originalColor, currentColor);
            (int newR, int newG, int newB) = Color.Diff(originalColor, newColor);
            int rDiff = newR * newR - currR * currR,
                gDiff = newG * newG - currG * currG,
                bDiff = newB * newB - currB * currB;

            int penalty = 1;

            if (rDiff > 0) rDiff *= penalty;
            if (gDiff > 0) gDiff *= penalty;
            if (bDiff > 0) bDiff *= penalty;

            return rDiff + gDiff + bDiff;
        }
    }
}