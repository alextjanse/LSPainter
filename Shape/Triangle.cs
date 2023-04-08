using LSPainter.Maths;

namespace LSPainter.Shapes
{
    public class Triangle : Shape
    {
        public Vector P1 { get; }
        public Vector P2 { get; }
        public Vector P3 { get; }

        public Triangle(Vector p1, Vector p2, Vector p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;

            Area = Vector.Determinant(P1, P2, P3);

            double minX = Math.Min(Math.Min(P1.X, P2.X), P3.X);
            double maxX = Math.Max(Math.Max(P1.X, P2.X), P3.X);
            double minY = Math.Min(Math.Min(P1.Y, P2.Y), P3.Y);
            double maxY = Math.Max(Math.Max(P1.Y, P2.Y), P3.Y);

            BoundingBox = new Rectangle(minX, minY, maxX - minX, maxY - minY);
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
    }
}