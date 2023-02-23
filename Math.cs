namespace LSPainter
{
    public struct Point
    {
        public float X, Y;

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Vector(Point p) => new Vector(p.X, p.Y);
    }

    public struct Vector
    {
        public float X, Y;

        public Vector(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Point(Vector v) => new Point(v.X, v.Y);

        public float Length => (float)Math.Sqrt(X * X + Y * Y);

        public static Vector operator +(Vector v) => v;
        public static Vector operator -(Vector v) => new Vector(-v.X, -v.Y);
        public static Vector operator +(Vector u, Vector v) => new Vector(u.X + v.X, u.Y + v.Y);
        public static Vector operator -(Vector u, Vector v) => new Vector(u.X - v.X, u.Y - v.Y);
        public static Vector operator *(float f, Vector v) => new Vector(f * v.X, f * v.Y);
        public static Vector operator *(Vector v, float f) => f * v;

        public static float Dot(Vector u, Vector v) => u.X * v.X + u.Y * v.Y;
        public static float edgeFunction(Vector a, Vector b, Vector c) => (c.X - a.X) * (b.Y - a.Y) - (c.Y - a.Y) * (b.X - a.X);
    }

    public struct Rectangle
    {
        public float X, Y, Width, Height;
        public static implicit operator BoundingBox(Rectangle r) => new BoundingBox(
                                                                        (int)Math.Floor(r.X),
                                                                        (int)Math.Floor(r.Y),
                                                                        (int)Math.Ceiling(r.Width),
                                                                        (int)Math.Ceiling(r.Height)
                                                                    );

        public Rectangle(float x, float y, float width, float height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }

    public struct BoundingBox{
        public int X, Y, Width, Height;

        public BoundingBox(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    }
}