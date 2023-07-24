using System.Collections;

namespace LSPainter.Maths
{
    public interface IBoundable
    {
        Rectangle BoundingBox { get; }
    }

    public class Rectangle : IEnumerable<(int, int)>
    {

        public double MinX { get; private set; }
        public double MinY { get; private set; }
        public double MaxX { get; private set; }
        public double MaxY { get; private set; }

        public double Width => MaxX - MinX;
        public double Height => MaxY - MinY;

        public static Rectangle Empty => new Rectangle(double.NaN, double.NaN, double.NaN, double.NaN);
        public bool IsEmpty => MinX + MaxX + MinY + MaxY == double.NaN;

        public Rectangle(double minX, double maxX, double minY, double maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }

        public static Rectangle Intersect(Rectangle a, Rectangle b)
        {
            if (a.IsEmpty || b.IsEmpty) return Rectangle.Empty;

            double xMin = Math.Max(a.MinX, b.MinX);
            double xMax = Math.Min(a.MaxX, b.MaxX);
            double yMin = Math.Max(a.MinY, b.MinY);
            double yMax = Math.Min(a.MaxY, b.MaxY);

            return new Rectangle(xMin, xMax, yMin, yMax);
        }

        public bool Overlaps(Rectangle other)
        {
            if (IsEmpty || other.IsEmpty)
            {
                // Double.NaN comparisons always evaluate false, so other checks malfunction
                return false;
            }

            if (MaxX < other.MinX || MinX > other.MaxX)
            {
                // Check if this bounding box isn't out of the other's x-range
                return false;
            }

            if (MaxY < other.MinY || MinY > other.MaxY)
            {
                // Same for y-range
                return false;
            }

            // Their x-range and y-range overlap. They must overlap.
            return true;
        }

        public bool In(Rectangle other)
        {
            return other.MinX <= MinX
                && other.MaxX >= MaxX
                && other.MinY <= MinY
                && other.MaxY >= MaxY;
        }

        public static Rectangle Union(Rectangle a, Rectangle b)
        {
            double minX = Math.Min(a.MinX, b.MinX);
            double maxX = Math.Max(a.MaxX, b.MaxX);
            double minY = Math.Min(a.MinY, b.MinY);
            double maxY = Math.Max(a.MaxY, b.MaxY);

            return new Rectangle(minX, maxX, minY, maxY);
        }

        public void UnionWith(Rectangle other)
        {
            double minX = Math.Min(MinX, other.MinX);
            double maxX = Math.Max(MaxX, other.MaxX);
            double minY = Math.Min(MinY, other.MinY);
            double maxY = Math.Max(MaxY, other.MaxY);
        }

        public static Rectangle FromPointCloud(IEnumerable<Vector> points)
        {
            double minX = double.PositiveInfinity;
            double maxX = double.NegativeInfinity;
            double minY = double.PositiveInfinity;
            double maxY = double.NegativeInfinity;

            foreach (Vector v in points)
            {
                minX = Math.Min(minX, v.X);
                maxX = Math.Max(maxX, v.X);
                minY = Math.Min(minY, v.Y);
                maxY = Math.Max(maxY, v.Y);
            }

            return new Rectangle(minX, maxX, minY, maxY);
        }

        public IEnumerator GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator<(int, int)> IEnumerable<(int, int)>.GetEnumerator()
        {
            int minX = (int)Math.Floor(MinX);
            int maxX = (int)Math.Ceiling(MaxX);
            int minY = (int)Math.Floor(MinY);
            int maxY = (int)Math.Ceiling(MaxY);

            for (int y = minY; y < maxY; y++)
            {
                for (int x = minX; x < maxX; x++)
                {
                    yield return (x, y);
                }
            }
        }
    }
}