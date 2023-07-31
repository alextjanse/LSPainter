namespace LSPainter.Maths
{
    public class Circle : Shape
    {
        public override Rectangle BoundingBox { get; protected set; }

        public Vector Origin { get; set; }
        public double Radius { get; set; }

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

        Rectangle CreateBoundingBox()
        {
            double minX = Math.Floor(Origin.X - Radius);
            double maxX = Math.Ceiling(Origin.X + Radius);
            double minY = Math.Floor(Origin.Y - Radius);
            double maxY = Math.Ceiling(Origin.Y + Radius);

            return new Rectangle(minX, maxX, minY, maxY);
        }

        public override void Translate(Vector translation)
        {
            Origin += translation;

            BoundingBox = CreateBoundingBox();
        }

        public override object Clone()
        {
            return new Circle((Vector)Origin.Clone(), Radius);
        }
    }
}