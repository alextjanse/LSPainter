namespace LSPainter.LSSolver.Painter
{
    public abstract class CanvasScore<TSolution> : Score<TSolution> where TSolution : CanvasSolution
    {
        public long SquaredPixelDiff { get; set; }

        public CanvasScore(long squaredPixelDiff)
        {
            SquaredPixelDiff = squaredPixelDiff;
        }

        public override double ToDouble()
        {
            return (double)SquaredPixelDiff;
        }
    }
}