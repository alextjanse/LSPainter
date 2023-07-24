using LSPainter.Maths;

namespace LSPainter.LSSolver.Painter
{
    public abstract class CanvasOperation<TSolution, TScore, TChecker> : Operation<TSolution, TScore, TChecker> where TSolution : CanvasSolution where TScore : CanvasScore<TSolution> where TChecker : CanvasChecker<TSolution, TScore>
    {
        public Rectangle BBox { get; protected set; }

        public CanvasOperation(Rectangle bbox)
        {
            BBox = bbox;
        }
        
        public Vector GetPixelVector(int x, int y)
        {
            return new Vector(x + 0.5, y + 0.5);
        }

        protected void TrimToCanvas(TChecker checker)
        {
            BBox = Rectangle.Intersect(BBox, checker.OriginalImage.BBox);
        }
    }
}