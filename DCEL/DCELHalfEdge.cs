using LSPainter.Maths;

namespace LSPainter.DCEL
{
    public class DCELHalfEdge
    {
        public static explicit operator LineSegment(DCELHalfEdge? e) => new LineSegment(
            (Vector)(e?.Origin ?? throw new NullReferenceException()),
            (Vector)(e?.Next?.Origin ?? throw new NullReferenceException())
        );
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

        public DCELVertex? Origin { get; set; }
        public DCELHalfEdge? Twin { get; set; }        
        public DCELFace? IncidentFace { get; set; }
        public DCELHalfEdge? Next { get; set; }
        public DCELHalfEdge? Prev { get; set; }

        public DCELHalfEdge()
        {

        }

        public DCELHalfEdge(DCELHalfEdge prev, DCELVertex origin, DCELHalfEdge next)
        {
            Prev = prev;
            Origin = Origin;
            Next = next;
        }

        public DCELHalfEdge(DCELVertex origin, DCELHalfEdge twin, DCELFace incidentFace, DCELHalfEdge next, DCELHalfEdge prev)
        {
            Origin = origin;
            Twin = twin;
            IncidentFace = incidentFace;
            Next = next;
            Prev = prev;
        }

        public void SetOrigin(DCELVertex origin)
        {
            Origin = origin;
        }

        public void SetTwinAndItsTwin(DCELHalfEdge twin)
        {
            Twin = twin;
            twin.Twin = this;
        }

        public void SetIncidentFace(DCELFace incidentFace)
        {
            IncidentFace = incidentFace;
        }

        public void SetNextAndItsPrev(DCELHalfEdge next)
        {
            Next = next;
            next.Prev = this;
        }

        public void SetPrevAndItsNext(DCELHalfEdge prev)
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