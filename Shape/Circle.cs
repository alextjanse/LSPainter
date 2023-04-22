using LSPainter.Maths;

namespace LSPainter.Shapes
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

            BoundingBox = new Rectangle(Origin.X - Radius, Origin.Y - Radius, Radius * 2, Radius * 2);
        }

        public override bool IsInside(Vector p)
        {
            return (p - Origin).Length <= Radius;
        }
    }
}