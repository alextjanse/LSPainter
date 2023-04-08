namespace LSPainter.Maths
{
    public class Triangle
    {
        public Vertex P1 { get; }
        public Vertex P2 { get; }
        public Vertex P3 { get; }

        public Triangle(Vertex p1, Vertex p2, Vertex p3)
        {
            P1 = p1;
            P2 = p2;
            P3 = p3;
        }
    }
}