namespace LSPainter.Maths
{
    public class Triangle : Shape
    {
        public Vector P1 { get; private set; }
        public Vector P2 { get; private set; }
        public Vector P3 { get; private set; }

        public override double Area => Vector.Determinant(P1, P2, P3);
        public override Rectangle BoundingBox => Rectangle.FromPointCloud(new[] { P1, P2, P3 });
        public override Vector Centroid => (P1 + P2 + P3) * (1.0 / 3.0);

        public Triangle(Vector p1, Vector p2, Vector p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }

        public override bool IsInside(Vector p)
        {
            // Source: https://www.scratchapixel.com/lessons/3d-basic-rendering/rasterization-practical-implementation/rasterization-stage.html
            
            return IsRightOfHalfEdge(P2, P3, p) &&
                   IsRightOfHalfEdge(P3, P1, p) &&
                   IsRightOfHalfEdge(P1, P2, p);
        }

        bool IsRightOfHalfEdge(Vector v1, Vector v2, Vector p)
        {
            return Vector.Determinant(v1, v2, p) >= 0;
        }

        public override void Translate(Vector translation)
        {
            P1 += translation;
            P2 += translation;
            P3 += translation;
        }

        public override void Resize(double scale)
        {
            Vector centroid = Centroid;

            Vector p1c = P1 - centroid;
            Vector p2c = P2 - centroid;
            Vector p3c = P3 - centroid;

            p1c *= scale;
            p2c *= scale;
            p3c *= scale;

            P1 = centroid + p1c;
            P2 = centroid + p2c;
            P3 = centroid + p3c;
        }

        public override object Clone()
        {
            return new Triangle((Vector)P1.Clone(), (Vector)P2.Clone(), (Vector)P3.Clone());
        }

        public override string ToString() => $"Triangle: p1={P1}, p2={P2}, p3={P3}";
    }
}