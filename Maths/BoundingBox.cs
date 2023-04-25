namespace LSPainter.Maths
{
    public struct BoundingBox
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
    }
}