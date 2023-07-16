using System.Collections;

namespace LSPainter.Maths
{
    public struct BoundingBox : IEnumerable<(int, int)>
    {
        public int X, Y, Width, Height;

        public BoundingBox(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
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

            return new BoundingBox(minX, minY, maxX - minX, maxY - minY);
        }

        public IEnumerator GetEnumerator()
        {
            return this.GetEnumerator();
        }

        IEnumerator<(int, int)> IEnumerable<(int, int)>.GetEnumerator()
        {
            for (int y = Y; y < Y + Height; y++)
            {
                for (int x = X; x < X + Width; x++)
                {
                    yield return (x, y);
                }
            }
        }
    }
}