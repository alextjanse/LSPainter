using System.Collections;

using LSPainter.Maths.DCEL;

namespace LSPainter.Maths
{
    public struct BoundingBox : IEnumerable<(int x, int y)>
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

        IEnumerator<(int, int)> IEnumerable<(int x, int y)>.GetEnumerator()
        {
            for (int y = Y; y < Y + Height; y++)
            {
                for (int x = X; x < X + Width; x++)
                {
                    yield return (x, y);
                }
            }
        }

        public static BoundingBox FromFace(Face face)
        {
            int minX = int.MaxValue;
            int maxX = int.MinValue;
            int minY = int.MaxValue;
            int maxY = int.MinValue;

            foreach (Vertex vertex in face.Vertices)
            {
                double x = vertex.X;
                double y = vertex.Y;

                minX = Math.Min(minX, (int)Math.Floor(x));
                maxX = Math.Max(maxX, (int)Math.Ceiling(x));
                minY = Math.Min(minY, (int)Math.Floor(y));
                maxY = Math.Max(maxY, (int)Math.Ceiling(y));
            }

            int width = maxX - minX;
            int height = maxY - minY;

            return new BoundingBox(minX, minY, width, height);
        }
    }
}