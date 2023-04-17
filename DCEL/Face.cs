using System.Collections;

namespace LSPainter.DCEL
{
    public class Face : IEnumerable<HalfEdge>, IEquatable<Face>
    {
        public static bool operator ==(Face? f, Face? g)
        {
            if (f is null) return g is null;
            return f.Equals(g);
        }

        public static bool operator !=(Face? f, Face? g) => !(f == g);

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

        public HalfEdge? OuterComponent { get; set; }
        
        public List<HalfEdge> InnerComponents;

        public Face()
        {
            InnerComponents = new List<HalfEdge>();
        }

        public void AddInnerComponent(HalfEdge innerComponent)
        {
            InnerComponents.Add(innerComponent);
        }

        public void SetOuterComponent(HalfEdge? outerComponent)
        {
            OuterComponent = outerComponent;
        }

        public override string ToString()
        {
            return $"Face {ID}";
        }

        public Face Clone()
        {
            /* 
            Make a new Face with all it's incident edges and vertices, but
            without the adjacent faces. We can use this to copy a DCEL for
            triangulation and what not.

            Loop over all edges around the face, and copy their data in new
            half-edges and vertices. Set all pointer data correct and so,
            and we're done.
             */
            Face faceClone = new Face();

            HalfEdge startEdge = OuterComponent ?? throw new NullReferenceException();
            
            // Clone the start edge, so we can link off of that
            HalfEdge headEdgesClone = startEdge.Clone();
            headEdgesClone.SetIncidentFace(faceClone);

            faceClone.SetOuterComponent(headEdgesClone);

            // Keep track of the tail of the tail of the edge chain
            HalfEdge tailEdgesClone = headEdgesClone;
            
            // Because we did the first one seperate, start with the next edge
            HalfEdge currentEdge = startEdge.Next ?? throw new NullReferenceException();

            do
            {
                // Make a clone of the current edge
                HalfEdge edgeClone = currentEdge.Clone();
                edgeClone.SetIncidentFace(faceClone);

                // Chain it to the tail of the edge chain and set as tail
                tailEdgesClone.AppendHalfEdge(edgeClone);

                tailEdgesClone = edgeClone;

                currentEdge = currentEdge.Next ?? throw new NullReferenceException();
            }
            while (currentEdge != startEdge);

            // Tie the head and tail together
            tailEdgesClone.AppendHalfEdge(headEdgesClone);

            return faceClone;
        }

        public IEnumerator<HalfEdge> GetEnumerator()
        {
            return new EdgeEnumerator(OuterComponent ?? throw new NullReferenceException());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool Equals(Face? other)
        {
            if (other == null) return false;

            foreach ((HalfEdge edge, HalfEdge otherEdge) in Enumerable.Zip<HalfEdge, HalfEdge>(this, other))
            {
                if (edge != otherEdge) return false;
            }

            return true;
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

            return this.Equals(obj as Face);
        }

        public override int GetHashCode()
        {
            return (ID, this.Select(edge => edge.GetHashCode())).GetHashCode();
        }
    }

    class EdgeEnumerator : IEnumerator<HalfEdge>
    {
        private HalfEdge currentEdge, startEdge;

        public HalfEdge Current => currentEdge;

        object IEnumerator.Current => currentEdge;

        public EdgeEnumerator(HalfEdge startEdge)
        {
            this.startEdge = startEdge;
            currentEdge = startEdge;
        }

        public bool MoveNext()
        {
            currentEdge = currentEdge.Next ?? throw new NullReferenceException();

            return currentEdge != startEdge;
        }

        public void Reset()
        {
            currentEdge = startEdge;
        }

        public void Dispose()
        {
            // Nothing
        }
    }

    class VertexEnumerator : IEnumerator<Vertex>
    {
        private HalfEdge currentEdge, startEdge;

        public Vertex Current => currentEdge.Origin ?? throw new NullReferenceException();

        object IEnumerator.Current => currentEdge;

        public VertexEnumerator(HalfEdge startEdge)
        {
            currentEdge = startEdge;
            this.startEdge = startEdge;
        }

        public bool MoveNext()
        {
            currentEdge = currentEdge.Next ?? throw new NullReferenceException();

            return currentEdge != startEdge;
        }

        public void Reset()
        {
            currentEdge = startEdge;
        }

        public void Dispose()
        {
            // Nothing
        }
    }
}