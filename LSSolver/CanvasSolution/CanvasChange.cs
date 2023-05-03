using LSPainter.Maths;

namespace LSPainter.LSSolver.Canvas
{
    public abstract class CanvasChange : IChange
    {
        public abstract BoundingBox BoundingBox { get; }
        public abstract Color GetPixel(int x, int y, Color currentColor);
    }
}