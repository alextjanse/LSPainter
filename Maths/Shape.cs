namespace LSPainter.Maths
{
    public abstract class Shape : IBoundable, ICloneable
    {
        public abstract Rectangle BoundingBox { get; protected set; }

        public double Area { get; protected set; }
        public abstract bool IsInside(Vector p);
        public bool IsInside(int x, int y)
        {
            Vector p = new Vector(x + 0.5, y + 0.5);

            return IsInside(p);
        }

        public abstract void Translate(Vector translation);

        public abstract object Clone();
    }
}