using LSPainter.Maths;

namespace LSPainter.LSSolver.Painter
{
    public abstract class CanvasOperation<TSolution, TScore, TChecker> : Operation<TSolution, TScore, TChecker> where TSolution : CanvasSolution where TScore : CanvasScore<TSolution> where TChecker : CanvasChecker<TSolution, TScore>
    {
        public Rectangle BoudningBox { get; protected set; }

        public CanvasOperation(Rectangle boundingBox)
        {
            BoudningBox = boundingBox;
        }
        
        public Vector GetPixelVector(int x, int y)
        {
            return new Vector(x + 0.5, y + 0.5);
        }

        protected void TrimToCanvas(TChecker checker)
        {
            BoudningBox = Rectangle.Intersect(BoudningBox, checker.OriginalImage.BBox);
        }
    }
}