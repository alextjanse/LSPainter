namespace LSPainter.Maths.Shapes
{
    public abstract class Shape : IBoundable
    {
        public abstract Rectangle BoundingBox { get; }

        public double Area { get; protected set; }
        public abstract bool IsInside(Vector p);
        public bool IsInside(int x, int y)
        {
            Vector p = new Vector(x + 0.5, y + 0.5);

            return IsInside(p);
        }
    }
}