namespace LSPainter.Maths
{
    public abstract class Shape : IBoundable, ICloneable
    {
        public enum Type { Circle, Triangle };

        public abstract Rectangle BoundingBox { get; }
        public abstract Vector Centroid { get; }

        public abstract double Area { get; }
        public abstract bool IsInside(Vector p);
        public bool IsInside(int x, int y)
        {
            Vector p = new Vector(x + 0.5, y + 0.5);

            return IsInside(p);
        }

        public abstract void Translate(Vector translation);
        public abstract void Resize(double scale);

        public abstract object Clone();
    }
}