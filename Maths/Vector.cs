namespace LSPainter.Maths
{
    public class Vector : IComparable<LineSegment>, IEquatable<Vector>
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
        public static float Determinant(Vector u, Vector v, Vector w) => (w.X - u.X) * (v.Y - u.Y) - (w.Y - u.Y) * (v.X - u.X);

        public static Vector UnitX = new Vector(1, 0);
        public static Vector UnitY = new Vector(0, 1);

        public void Normalize()
        {
            if (X * X + Y * Y == 1) return; // Already normalized

            float factor = 1 / Length;

            X *= factor;
            Y *= factor;
        }

        public Vector Normalized()
        {
            if (X * X + Y * Y == 1) return new Vector(X, Y); // Already normalized

            float factor = 1 / Length;

            return new Vector(X * factor, Y * factor);
        }

        public static Vector Normalized(Vector v)
        {
            return v.Normalized();
        }

        public int CompareTo(LineSegment l)
        {
            return ((Point)this).CompareTo(l);
        }

        public static bool operator ==(Vector? u, Vector? v)
        {
            if (u == null)
            {
                return v == null; // null == null = true
            }
            
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
    }
}