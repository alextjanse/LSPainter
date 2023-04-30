using LSPainter.Maths;

namespace LSPainter.Solver
{
    public abstract class Change
    {
        public abstract BoundingBox GenerateBoundingBox();
    }
}