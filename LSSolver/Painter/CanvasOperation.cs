using LSPainter.Maths;

namespace LSPainter.LSSolver.Painter
{
    public abstract class CanvasOperation<TSolution, TScore, TChecker> : Operation<TSolution, TScore, TChecker> where TSolution : CanvasSolution where TScore : CanvasScore<TSolution> where TChecker : CanvasChecker<TSolution, TScore>
    {
        public Rectangle BoundingBox { get; protected set; }
        protected CanvasSketch Sketch { get; }

        public CanvasOperation(Rectangle boundingBox)
        {
            BoundingBox = boundingBox;
            Sketch = new CanvasSketch(boundingBox);
        }
        
        public Vector GetPixelVector(int x, int y)
        {
            return new Vector(x + 0.5, y + 0.5);
        }

        protected void TrimToCanvas(TChecker checker)
        {
            BoundingBox = Rectangle.Intersect(BoundingBox, checker.OriginalImage.BBox);
        }
    }
}