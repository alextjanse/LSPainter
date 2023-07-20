using LSPainter.Maths;

namespace LSPainter.Maths.Shapes
{
    public class Circle : Shape
    {
        public Vector Origin { get; }
        public double Radius { get; }

        public Circle(Vector origin, double radius)
        {
            Origin = origin;
            Radius = radius;

            Area = Math.PI * Radius * Radius;
        }

        public override bool IsInside(Vector p)
        {
            return (p - Origin).Length <= Radius;
        }

        public override BoundingBox CreateBoundingBox()
        {
            int minX = (int)Math.Floor(Origin.X - Radius);
            int maxX = (int)Math.Ceiling(Origin.X + Radius);
            int minY = (int)Math.Floor(Origin.Y - Radius);
            int maxY = (int)Math.Ceiling(Origin.Y + Radius);

            return new BoundingBox(minX, maxX, minY, maxY);
        }
    }
}