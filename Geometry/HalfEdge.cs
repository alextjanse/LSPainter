namespace LSPainter.Geometry
{
    public class HalfEdge
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

        public Vertex? Origin { get; set; }
        public HalfEdge? Twin { get; set; }        
        public Face? IncidentFace { get; set; }
        public HalfEdge? Next { get; set; }
        public HalfEdge? Prev { get; set; }

        public HalfEdge()
        {

        }

        public HalfEdge(HalfEdge prev, Vertex origin, HalfEdge next)
        {
            Prev = prev;
            Origin = Origin;
            Next = next;
        }

        public HalfEdge(Vertex origin, HalfEdge twin, Face incidentFace, HalfEdge next, HalfEdge prev)
        {
            Origin = origin;
            Twin = twin;
            IncidentFace = incidentFace;
            Next = next;
            Prev = prev;
        }

        public void SetOrigin(Vertex origin)
        {
            Origin = origin;
        }

        public void SetTwin(HalfEdge twin)
        {
            Twin = twin;
            twin.Twin = this;
        }

        public void SetIncidentFace(Face incidentFace)
        {
            IncidentFace = incidentFace;
        }

        public void SetNext(HalfEdge next)
        {
            Next = next;
            next.Prev = this;
        }

        public void SetPrev(HalfEdge prev)
        {
            Prev = prev;
            prev.Next = this;
        }

        public override string ToString()
        {
            return $"Half-edge {ID}";
        }
    }
}