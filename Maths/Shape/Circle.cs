using LSPainter.Maths;

namespace LSPainter.Maths.Shapes
{
    public class Circle : Shape
    {
        public override Rectangle BoundingBox { get; }

        public Vector Origin { get; }
        public double Radius { get; }

        public Circle(Vector origin, double radius)
        {
            Origin = origin;
            Radius = radius;

            Area = Math.PI * Radius * Radius;

            BoundingBox = CreateBoundingBox();
        }

        public override bool IsInside(Vector p)
        {
            return (p - Origin).Length <= Radius;
        }

        public Rectangle CreateBoundingBox()
        {
            double minX = Math.Floor(Origin.X - Radius);
            double maxX = Math.Ceiling(Origin.X + Radius);
            double minY = Math.Floor(Origin.Y - Radius);
            double maxY = Math.Ceiling(Origin.Y + Radius);

            return new Rectangle(minX, maxX, minY, maxY);
        }
    }
}