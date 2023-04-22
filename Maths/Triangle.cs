namespace LSPainter.Maths
{
    public class Triangle
    {
        public Vector P1 { get; }
        public Vector P2 { get; }
        public Vector P3 { get; }

        public Triangle(Vector p1, Vector p2, Vector p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }
    }
}