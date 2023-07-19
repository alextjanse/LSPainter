using System.Collections;

namespace LSPainter.Maths
{
    public class BoundingBox : IEnumerable<(int, int)>
    {
        public static BoundingBox Intersect(BoundingBox a, BoundingBox b)
        {
            int xMin = Math.Max(a.MinX, b.MinX);
            int xMax = Math.Min(a.MaxX, b.MaxX);
            int yMin = Math.Max(a.MinY, b.MinY);
            int yMax = Math.Min(a.MaxY, b.MaxY);

            return new BoundingBox(xMin, xMax, yMin, yMax);
        }

        public int MinX { get; }
        public int MinY { get; }
        public int MaxX { get; }
        public int MaxY { get; }

        public BoundingBox(int minX, int maxX, int minY, int maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;
        }

        public static BoundingBox FromPointCloud(IEnumerable<Vector> points)
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;

            foreach (Vector v in points)
            {
                minX = Math.Min(minX, (int)Math.Floor(v.X));
                maxX = Math.Max(maxX, (int)Math.Ceiling(v.X));
                minY = Math.Min(minY, (int)Math.Floor(v.Y));
                maxY = Math.Max(maxY, (int)Math.Ceiling(v.Y));
            }

            return new BoundingBox(minX, maxX, minY, maxY);
        }

        public IEnumerator GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator<(int, int)> IEnumerable<(int, int)>.GetEnumerator()
        {
            for (int y = MinY; y < MaxY; y++)
            {
                for (int x = MinX; x < MaxX; x++)
                {
                    yield return (x, y);
                }
            }
        }
    }
}