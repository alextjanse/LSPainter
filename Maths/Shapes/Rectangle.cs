namespace LSPainter.Maths
{
    public interface IBoundable
    {
        Rectangle BoundingBox { get; }
    }

    public class Rectangle : Shape
    {

        public static Rectangle Empty => new Rectangle(double.NaN, double.NaN, double.NaN, double.NaN);

        public double MinX { get; private set; }
        public double MinY { get; private set; }
        public double MaxX { get; private set; }
        public double MaxY { get; private set; }

        public double Width => MaxX - MinX;
        public double Height => MaxY - MinY;

        public bool IsEmpty => MinX + MaxX + MinY + MaxY == double.NaN;

        public override Rectangle BoundingBox
        {
            get
            {
                return this;
            }
            protected set
            {
                MinX = value.MinX;
                MaxX = value.MaxX;
                MinY = value.MinY;
                MaxY = value.MaxY;
                Area = value.Area;
            }
        }

        public override Vector Centroid => new Vector(MinX + (MaxX - MinX) / 2, MinY + (MaxY - MinY) / 2);

        public Rectangle(double minX, double maxX, double minY, double maxY)
        {
            MinX = minX;
            MaxX = maxX;
            MinY = minY;
            MaxY = maxY;

            Area = Width * Height;
            BoundingBox = this;
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

        public static Rectangle Intersect(Rectangle a, Rectangle b)
        {
            if (a.IsEmpty || b.IsEmpty) return Rectangle.Empty;

            double xMin = Math.Max(a.MinX, b.MinX);
            double xMax = Math.Min(a.MaxX, b.MaxX);
            double yMin = Math.Max(a.MinY, b.MinY);
            double yMax = Math.Min(a.MaxY, b.MaxY);

            // Intersection is empty
            if (xMin > xMax || yMin > yMax) return Rectangle.Empty;

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

        public IEnumerable<(int x, int y)> PixelCoords()
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

        public (int XOffset, int YOffset) OriginOffsets => ((int)Math.Floor(MinX), (int)Math.Floor(MinY));
        public int SectionWidth => (int)Math.Ceiling(MaxX) - (int)Math.Floor(MinX);
        public int SectionHeight => (int)Math.Ceiling(MaxY) - (int)Math.Floor(MinY);

        public override object Clone()
        {
            return new Rectangle(MinX, MaxX, MinY, MaxY);
        }

        public override bool IsInside(Vector p)
        {
            return (
                MinX <= p.X && p.X <= MaxX &&
                MinY <= p.Y && p.Y <= MaxY
            );
        }

        public override void Translate(Vector translation)
        {
            MinX += translation.X;
            MaxX += translation.X;
            MinY += translation.Y;
            MaxY += translation.Y;
        }

        public override void Resize(double scale)
        {
            Vector centroid = Centroid;

            double newWidth = Width * scale;
            double newHeight = Height * scale;

            MinX = centroid.X - newWidth / 2;
            MaxX = centroid.X + newWidth / 2;
            MinY = centroid.Y - newHeight / 2;
            MaxY = centroid.Y + newHeight / 2;

            Area = newWidth * newHeight;
        }
    }
}