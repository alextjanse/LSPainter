namespace LSPainter.Shapes
{
    public abstract class Shape
    {
        public Rectangle BoundingBox { get; protected set; }
        public float Area { get; protected set; }
        public abstract bool IsInside(Vector p);
        public bool IsInside(int x, int y)
        {
            Vector p = new Vector(x + 0.5f, y + 0.5f);

            return IsInside(p);
        }
    }
}