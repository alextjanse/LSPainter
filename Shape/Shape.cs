namespace LSPainter.Shapes
{
    public abstract class Shape
    {
        public Rectangle BoundingBox { get; protected set; }
        public float Area { get; protected set; }
        public abstract bool IsInside(Vector p);
    }
}