using LSPainter.Maths;

namespace LSPainter.LSSolver.Painter
{
    public abstract class CanvasOperationFactory<TSolution, TScore, TChecker> : OperationFactory<TSolution, TScore, TChecker> where TSolution : CanvasSolution where TScore : CanvasScore<TSolution> where TChecker : CanvasChecker<TSolution, TScore>
    {
        public int CanvasWidth { get; }
        public int CanvasHeight { get; }

        public CanvasOperationFactory(int canvasWidth, int canvasHeight)
        {
            CanvasWidth = canvasWidth;
            CanvasHeight = canvasHeight;
        }

        public Rectangle TrimToCanvas(Rectangle boundingBox)
        {
            double minX = Math.Max(0, boundingBox.MinX);
            double maxX = Math.Min(CanvasWidth, boundingBox.MaxX);
            double minY = Math.Max(0, boundingBox.MinY);
            double maxY = Math.Min(CanvasHeight, boundingBox.MaxY);

            return new Rectangle(minX, maxX, minY, maxY);
        }
    }
}