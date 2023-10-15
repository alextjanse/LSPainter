namespace LSPainter.Maths
{
    public class Vector : IComparable<LineSegment>, IEquatable<Vector>, ICloneable
    {
        public double X { get; private set; }
        public double Y { get; private set; }
        public double Length => Math.Sqrt(X * X + Y * Y);

        public Vector(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static Vector operator +(Vector v) => v;
        public static Vector operator -(Vector v) => new Vector(-v.X, -v.Y);
        public static Vector operator +(Vector u, Vector v) => new Vector(u.X + v.X, u.Y + v.Y);
        public static Vector operator -(Vector u, Vector v) => new Vector(u.X - v.X, u.Y - v.Y);
        public static Vector operator *(double f, Vector v) => new Vector(f * v.X, f * v.Y);
        public static Vector operator *(Vector v, double f) => f * v;
        public static double Dot(Vector u, Vector v) => u.X * v.X + u.Y * v.Y;
        public static double Cross(Vector u, Vector v) => (u.X * v.Y) - (u.Y * v.X);
        public static double Determinant(Vector u, Vector v, Vector w) => (w.X - u.X) * (v.Y - u.Y) - (w.Y - u.Y) * (v.X - u.X);

        public static Vector UnitX = new (1, 0);
        public static Vector UnitY = new (0, 1);
        
        public static Vector PixelPoint(int x, int y) => new (x + 0.5, y + 0.5);
        public static Vector UnitVector(double angle) => new(Math.Cos(angle), Math.Sin(angle));

        public void Normalize()
        {
            if (X * X + Y * Y == 1) return; // Already normalized

            double factor = 1 / Length;

            X *= factor;
            Y *= factor;
        }

        public Vector Normalized()
        {
            if (X * X + Y * Y == 1) return new Vector(X, Y); // Already normalized

            double factor = 1 / Length;

            return new Vector(X * factor, Y * factor);
        }

        public static Vector Normalized(Vector v)
        {
            return v.Normalized();
        }

        public int CompareTo(LineSegment l)
        {
            /* 
            Check if v lies left or right of line segment l, if we look from l1 to l2. Source:
            https://www.scratchapixel.com/lessons/3d-basic-rendering/rasterization-practical-implementation/rasterization-stage.html
             */

            double f = Vector.Determinant(l.V1, l.V2, this);

            if (f < 0)
            {
                return -1;
            }
            else if (f == 0)
            {
                return 0;
            }
            else
            {
                // f > 0
                return 1;
            }
        }

        public static bool operator ==(Vector? u, Vector? v)
        {
            if (u is null) return v is null;
            
            return u.Equals(v);
        }

        public static bool operator !=(Vector u, Vector v) => !(u == v);

        public override bool Equals(object? obj) => this.Equals(obj as Vector ?? throw new NullReferenceException());

        public override int GetHashCode() => (X, Y).GetHashCode();

        public bool Equals(Vector? other)
        {
            if (other == null) return false;

            return X == other.X && Y == other.Y;
        }

        public object Clone()
        {
            return new Vector(X, Y);
        }

        public override string ToString() => $"({X:F3}, {Y:F3})";
    }
}