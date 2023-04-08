namespace LSPainter.Maths
{
    public class Vertex : Vector
    {
        public Edge? Prev { get; set; }
        public Edge? Next { get; set; }

        public Vertex(double x, double y) : base(x, y)
        {
            
        }

        public void SetPrev(Edge? prev)
        {
            Prev = prev;
        }

        public void SetNext(Edge? next)
        {
            Next = next;
        }
    }
}