namespace LSPainter.LSSolver.Painter
{
    public abstract class CanvasScore<TSolution> : Score<TSolution> where TSolution : CanvasSolution
    {
        public long SquaredPixelDiff { get; set; }
        public long BlankPixels { get; set; }

        public CanvasScore(long squaredPixelDiff, long blankPixels)
        {
            SquaredPixelDiff = squaredPixelDiff;
            BlankPixels = blankPixels;
        }

        public override double ToDouble()
        {
            return (double)SquaredPixelDiff;
        }
    }
}