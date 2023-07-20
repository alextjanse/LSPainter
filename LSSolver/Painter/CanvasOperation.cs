using LSPainter.Maths;

namespace LSPainter.LSSolver.Painter
{
    public abstract class CanvasOperation<TSolution, TScore, TChecker> : Operation<TSolution, TScore, TChecker> where TSolution : CanvasSolution where TScore : CanvasScore<TSolution> where TChecker : CanvasChecker<TSolution, TScore>
    {
        public BoundingBox BBox { get; protected set; }

        public CanvasOperation(BoundingBox bbox)
        {
            BBox = bbox;
        }
        
        public Vector GetPixelVector(int x, int y)
        {
            return new Vector(x + 0.5, y + 0.5);
        }

        protected void TrimToCanvas(TChecker checker)
        {
            BBox = BoundingBox.Intersect(BBox, checker.OriginalImage.BBox);
        }
    }
}