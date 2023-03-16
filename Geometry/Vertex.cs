namespace LSPainter.Geometry
{
    public class Vertex
    {
        static uint idGen = 1;
        private uint id = 0;
        public uint ID
        {
            get
            {
                if (id == 0)
                {
                    id = idGen++;
                }
                return id;
            }
        }
        
        public float X { get; }
        public float Y { get; }

        public HalfEdge? IncidentEdge { get; set; }

        public Vertex(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void SetIncidentEdge(HalfEdge incidentEdge)
        {
            IncidentEdge = incidentEdge;
        }

        public override string ToString()
        {
            return $"Vertex {ID}";
        }
    }
}