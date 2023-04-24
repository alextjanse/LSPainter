using LSPainter.Maths;

namespace LSPainter.Maths.DCEL
{
    public class HalfEdge : IEquatable<HalfEdge>
    {
        public static explicit operator LineSegment(HalfEdge? e) => new LineSegment(
            (Vector)(e?.Origin ?? throw new NullReferenceException()),
            (Vector)(e?.Next?.Origin ?? throw new NullReferenceException())
        );

        public static bool operator ==(HalfEdge? i, HalfEdge? j)
        {
            if (i is null) return j is null;

            return i.Equals(j);
        }

        public static bool operator !=(HalfEdge? i, HalfEdge? j) => !(i == j);

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
            id = idGen++;
        }

        public void SetOrigin(Vertex? origin)
        {
            if (origin == null) throw new NullReferenceException();
            Origin = origin;
        }

        public void SetNext(HalfEdge? next)
        {
            if (next == null) throw new NullReferenceException();
            Next = next;
        }

        public void SetPrev(HalfEdge? prev)
        {
            if (prev == null) throw new NullReferenceException();
            Prev = prev;
        }

        public void SetNextAndItsPrev(HalfEdge? next)
        {
            if (next == null) throw new NullReferenceException();
            Next = next;
            next.Prev = this;
        }

        public void SetPrevAndItsNext(HalfEdge? prev)
        {
            if (prev == null) throw new NullReferenceException();
            Prev = prev;
            prev.Next = this;
        }

        public void SetTwin(HalfEdge? twin)
        {
            if (twin == null) throw new NullReferenceException();
            Twin = twin;
        }

        public void SetTwinAndItsTwin(HalfEdge twin)
        {
            Twin = twin;
            twin.Twin = this;
        }

        public void SetIncidentFace(Face? incidentFace)
        {
            if (incidentFace == null) throw new NullReferenceException();
            IncidentFace = incidentFace;
        }

        public void SetAsIncidentEdgeOfOrigin()
        {
            Origin?.SetIncidentEdge(this);
        }

        public void PrependHalfEdge(HalfEdge? prev)
        {
            SetPrevAndItsNext(prev);

            if (IncidentFace != null && prev?.IncidentFace == null) prev?.SetIncidentFace(IncidentFace);

            if (Twin?.Next == null)
            {
                /* 
                Tie the twins together as well. If twin.next != null, it means that
                it is already tied up, or there is some other edge going out of the
                common vertex, meaning that:

                            IncidentFace == prev.IncidentFace
                        Twin.IncidentFace != prev.Twin.IncidentFace
                
                In that case, we don't touch the twins at all.
                 */
                Twin?.AppendHalfEdge(prev?.Twin);
            }

            /* 
            It's fine to set this though. It might already be this value, so we could
            first check if the value is the same and then replace it. It is however
            cheaper to just set the value without checking. In either cases where the
            origin was already this value or not, the value at the end will be set to
            the correct origin.
             */
            if (Origin != null) prev?.Twin?.SetOrigin(Origin);
        }

        public void AppendHalfEdge(HalfEdge? next)
        {
            SetNextAndItsPrev(next);

            if (IncidentFace != null && next?.IncidentFace == null) next?.SetIncidentFace(IncidentFace);

            if (Twin?.Prev == null)
            {
                Twin?.PrependHalfEdge(next?.Twin ?? throw new NullReferenceException());
            }

            if (next?.Origin != null) Twin?.SetOrigin(next?.Origin);
        }

        public bool IsSetCorrectly()
        {
            if (Origin == null || Twin  == null || Next == null || Prev == null || IncidentFace == null) return false;

            if (!Origin.IsSetCorrectly()) return false;

            if (Next.Prev != this) return false;
            if (Prev.Next != this) return false;
            
            if (Twin.Twin != this) return false;
            if (Twin.Origin != Next.Origin) return false;

            return true;
        }

        public Vertex Destination()
        {
            return Twin?.Origin ?? throw new NullReferenceException();
        }

        public override string ToString()
        {
            return $"Half-edge {ID}: ({Origin}, {Destination()})";
        }

        public HalfEdge Clone()
        {
            /* 
            Clone a half-edge, by cloning its origin and setting the rest of the pointers to null
             */
            Vertex originClone = Origin?.Clone() ?? throw new NullReferenceException();

            HalfEdge edgeClone = new HalfEdge();
            edgeClone.SetOrigin(originClone);
            originClone.SetIncidentEdge(edgeClone);

            HalfEdge cloneTwin = new HalfEdge();
            edgeClone.SetTwinAndItsTwin(cloneTwin);

            return edgeClone;
        }

        public bool Equals(HalfEdge? other)
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

            return this.Equals(obj as HalfEdge);
        }

        public override int GetHashCode()
        {
            return (ID, Origin, Twin?.Origin).GetHashCode();
        }
    }
}