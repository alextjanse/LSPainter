namespace LSPainter.Maths
{
    public class Circle : Shape
    {
        public override string ToString() => $"Circle: p={Origin}, r={Radius:F3}";
        public Vector Origin { get; set; }
        public double Radius { get; set; }
        public override double Area => double.Pi * Radius * Radius;

        public override Rectangle BoundingBox => new(Origin.X - Radius,
                                                     Origin.X + Radius,
                                                     Origin.Y - Radius,
                                                     Origin.Y + Radius);
        public override Vector Centroid => Origin;

        public Circle(Vector origin, double radius)
        {
            Origin = origin;
            Radius = radius;
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
        }

        public override void Resize(double scale)
        {
            Radius *= scale;
        }

        public override object Clone()
        {
            return new Circle((Vector)Origin.Clone(), Radius);
        }
    }
}