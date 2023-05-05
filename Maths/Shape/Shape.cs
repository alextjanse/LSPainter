namespace LSPainter.Maths.Shapes
{
    public abstract class Shape
    {
        public abstract BoundingBox CreateBoundingBox();

        public double Area { get; protected set; }
        public abstract bool IsInside(Vector p);
        public bool IsInside(int x, int y)
        {
            Vector p = new Vector(x + 0.5, y + 0.5);

            return IsInside(p);
        }
        public IEnumerable<(int x, int y)> EnumeratePixels(int canvasWidth, int canvasHeight)
        {
            BoundingBox bbox = CreateBoundingBox();

            int minX = Math.Max(0, bbox.X);
            int minY = Math.Max(0, bbox.Y);
            int maxX = Math.Min(canvasWidth, bbox.X + bbox.Width);
            int maxY = Math.Min(canvasHeight, bbox.Y + bbox.Height);

            for (int y = minY; y < maxY; y++)
            {
                for (int x = minX; x < maxX; x++)
                {
                    if (IsInside(x, y))
                    {
                        yield return (x, y);
                    }
                }
            }
        }
    }
}