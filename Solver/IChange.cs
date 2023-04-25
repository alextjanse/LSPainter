using LSPainter.Maths;

namespace LSPainter.Solver
{
    public interface IChange
    {
        public BoundingBox GenerateBoundingBox();
        public void ApplyToCanvas(Painting canvas);
    }
}