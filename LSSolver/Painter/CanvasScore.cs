namespace LSPainter.LSSolver.Painter
{
    public class PlainCanvasConstraints : IConstraints<CanvasSolution>
    {
        public long EvaluateScore(IScore<CanvasSolution> score) => score.GetCleanScore();

        public void Update()
        {
            // Update parameters here.
        }
    }

    public class CanvasScore : IScore<CanvasSolution>
    {
        public static CanvasScore operator +(CanvasScore a, CanvasScore b) => new CanvasScore(a.PixelDiff + b.PixelDiff);

        public long PixelDiff { get; }

        public CanvasScore(long pixelDiff)
        {
            PixelDiff = pixelDiff;
        }

        public long GetCleanScore()
        {
            return PixelDiff;
        }

        public virtual IScore<CanvasSolution> Add(IScore<CanvasSolution> score)
        {
            CanvasScore other = (CanvasScore)score;

            return new CanvasScore(PixelDiff + other.PixelDiff);
        }
    }
}