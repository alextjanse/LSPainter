namespace LSPainter.Shapes
{
    public class Triangle : Shape
    {
        public Vector P1 { get; }
        public Vector P2 { get; }
        public Vector P3 { get; }

        public Vector[] Points { get; }

        public Triangle(Vector p1, Vector p2, Vector p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;

            Points = new Vector[] { P1, P2, P3 };

            float minX = float.MaxValue;
            float maxX = float.MinValue;
            float minY = float.MaxValue;
            float maxY = float.MinValue;

            Vector[] edges = new Vector[Points.Length];

            for (int i = 0; i < Points.Length; i++)
            {
                float pX = Points[i].X;
                float pY = Points[i].Y;

                minX = float.Min(minX, pX);
                maxX = float.Max(maxX, pX);
                minY = float.Min(minY, pY);
                maxY = float.Max(maxY, pY);

                edges[i] = Points[(i + 1) % Points.Length] - Points[i];
            }

            BoundingBox = new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        public override bool IsInside(Vector p)
        {
            float area = Vector.edgeFunction(P1, P2, P3);
            float w1 = Vector.edgeFunction(P2, P3, p);
            float w2 = Vector.edgeFunction(P3, P1, p);
            float w3 = Vector.edgeFunction(P1, P2, p);

            return (w1 >= 0 && w2 >= 0 && w3 >= 0);
        }
    }
}