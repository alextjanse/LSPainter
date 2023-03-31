namespace LSPainter
{
    public struct Point : IComparable<LineSegment>
    {
        public float X, Y;

        public Point(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Vector(Point p) => new Vector(p.X, p.Y);

        public int CompareTo(LineSegment l)
        {
            /* 
            Check if p lies left or right of line segment l, if we look from l1 to l2. Source:
            https://www.scratchapixel.com/lessons/3d-basic-rendering/rasterization-practical-implementation/rasterization-stage.html
             */
            
            float f = Vector.Determinant(l.V1, l.V2, p);

            if (f < 0)
            {
                return -1;
            }
            else if (f == 0)
            {
                return 0;
            }
            else {
                // f > 0
                return 1;
            }
        }
    }

    public struct Vector : IComparable<LineSegment>
    {
        public float X, Y;
        public float Length => (float)Math.Sqrt(X * X + Y * Y);

        public Vector(float x, float y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Point(Vector v) => new Point(v.X, v.Y);

        public static Vector operator +(Vector v) => v;
        public static Vector operator -(Vector v) => new Vector(-v.X, -v.Y);
        public static Vector operator +(Vector u, Vector v) => new Vector(u.X + v.X, u.Y + v.Y);
        public static Vector operator -(Vector u, Vector v) => new Vector(u.X - v.X, u.Y - v.Y);
        public static Vector operator *(float f, Vector v) => new Vector(f * v.X, f * v.Y);
        public static Vector operator *(Vector v, float f) => f * v;
        public static float Dot(Vector u, Vector v) => u.X * v.X + u.Y * v.Y;
        public static float Cross(Vector u, Vector v) => 
        public static float Determinant(Vector u, Vector v, Vector w) => (w.X - u.X) * (v.Y - u.Y) - (w.Y - u.Y) * (v.X - u.X);

        public static Vector UnitX = new Vector(1, 0);
        public static Vector UnitY = new Vector(0, 1);

        public int CompareTo(LineSegment l)
        {
            return ((Point)this).CompareTo(l);
        }
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

    public struct LineSegment
    {
        public Vector V1, V2;

        public LineSegment(Vector v1, Vector v2)
        {
            V1 = v1;
            V2 = v2;
        }

        public static LineSegment operator -(LineSegment l) => new LineSegment(l.V2, l.V1);

        public float GetXFromY(float y)
        {
            if (V2.Y - V1.Y == 0)
            {
                // Line segment is horizontal, just use an x-coord of either endpoint
                return V1.X;
            }

            float t = (y - V1.Y) / (V2.Y - V1.Y);

            if (0 <= t && t <= 1)
            {
                throw new Exception("not on line segment");
            }

            float x = V1.X + t * (V2.X - V1.X);

            return x;
        }
    }
}