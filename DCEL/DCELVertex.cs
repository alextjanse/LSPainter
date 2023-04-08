using LSPainter.Maths;

namespace LSPainter.DCEL
{
    public class DCELVertex
    {
        public static explicit operator Vector(DCELVertex v) => new Vector(v.X, v.Y);
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

        public DCELHalfEdge? IncidentEdge { get; set; }

        public DCELVertex(float x, float y)
        {
            X = x;
            Y = y;
        }

        public void SetIncidentEdge(DCELHalfEdge incidentEdge)
        {
            IncidentEdge = incidentEdge;
        }

        public override string ToString()
        {
            return $"Vertex {ID}";
        }
    }
}