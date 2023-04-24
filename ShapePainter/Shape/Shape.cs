using LSPainter.Maths;

namespace LSPainter.Shapes
{
    public abstract class Shape
    {
        public Rectangle BoundingBox { get; protected set; }
        public double Area { get; protected set; }
        public abstract bool IsInside(Vector p);
        public bool IsInside(int x, int y)
        {
            Vector p = new Vector(x + 0.5, y + 0.5);

            return IsInside(p);
        }
    }
}