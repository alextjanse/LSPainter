using LSPainter.Maths;

namespace LSPainter.LSSolver.Painter
{
    public abstract class CanvasOperation<TSolution, TScore, TChecker> : Operation<TSolution, TScore, TChecker> where TSolution : CanvasSolution where TScore : CanvasScore<TSolution> where TChecker : CanvasChecker<TSolution, TScore>
    {
        public BoundingBox BoundingBox { get; }

        public CanvasOperation(BoundingBox bbox)
        {
            BoundingBox = bbox;
        }
        
        public Vector GetPixelVector(int x, int y)
        {
            return new Vector(x + 0.5, y + 0.5);
        }
    }
}