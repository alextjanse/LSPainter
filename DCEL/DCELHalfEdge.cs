using LSPainter.Maths;

namespace LSPainter.DCEL
{
    public class DCELHalfEdge : IEquatable<DCELHalfEdge>
    {
        public static explicit operator LineSegment(DCELHalfEdge? e) => new LineSegment(
            (Vector)(e?.Origin ?? throw new NullReferenceException()),
            (Vector)(e?.Next?.Origin ?? throw new NullReferenceException())
        );

        public static bool operator ==(DCELHalfEdge? i, DCELHalfEdge? j)
        {
            if (i == null) return j == null;

            return i.Equals(j);
        }

        public static bool operator !=(DCELHalfEdge? i, DCELHalfEdge? j) => !(i == j);

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

        public DCELHalfEdge Clone()
        {
            /* 
            Clone a half-edge, by cloning its origin and setting the rest of the pointers to null
             */
            DCELVertex originClone = Origin?.Clone() ?? throw new NullReferenceException();

            DCELHalfEdge edgeClone = new DCELHalfEdge();
            edgeClone.SetOrigin(originClone);
            originClone.SetIncidentEdge(edgeClone);

            DCELHalfEdge cloneTwin = new DCELHalfEdge();
            edgeClone.SetTwinAndItsTwin(cloneTwin);

            return edgeClone;
        }

        public bool Equals(DCELHalfEdge? other)
        {
            if (other == null) return false;

            return other.Origin == Origin && other.Twin?.Origin == Twin?.Origin;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            return this.Equals(obj as DCELHalfEdge);
        }

        public override int GetHashCode()
        {
            return (Origin, Twin?.Origin).GetHashCode();
        }
    }
}