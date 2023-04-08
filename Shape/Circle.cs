using LSPainter.Maths;

namespace LSPainter.Shapes
{
    public class Circle : Shape
    {
        public Vector Origin { get; }
        public float Radius { get; }

        public Circle(Vector origin, float radius)
        {
            Origin = origin;
            Radius = radius;

            Area = (float)Math.PI * Radius * Radius;

            BoundingBox = new Rectangle(Origin.X - Radius, Origin.Y - Radius, Radius * 2, Radius * 2);
        }

        public override bool IsInside(Vector p)
        {
            return (p - Origin).Length <= Radius;
        }
    }
}