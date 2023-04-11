using System.Collections;

namespace LSPainter.DCEL
{
    public class DCELFace : IEnumerable<DCELHalfEdge>, IEquatable<DCELFace>
    {
        public static bool operator ==(DCELFace? f, DCELFace? g)
        {
            if (f == null) return g == null;
            return f.Equals(g);
        }

        public static bool operator !=(DCELFace? f, DCELFace? g) => !(f == g);

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

        public DCELHalfEdge? OuterComponent { get; set; }
        
        public List<DCELHalfEdge> InnerComponents;

        public DCELFace()
        {
            InnerComponents = new List<DCELHalfEdge>();
        }

        public void AddInnerComponent(DCELHalfEdge innerComponent)
        {
            InnerComponents.Add(innerComponent);
        }

        public void SetOuterComponent(DCELHalfEdge outerComponent)
        {
            OuterComponent = outerComponent;
        }

        public override string ToString()
        {
            return $"Face {ID}";
        }

        public DCELFace Clone()
        {
            /* 
            Make a new DCELFace with all it's incident edges and vertices, but
            without the adjacent faces. We can use this to copy a DCEL for
            triangulation and what not.

            Loop over all edges around the face, and copy their data in new half-
            edges and vertices. Set all pointer data correct and so, and we're done.
             */
            DCELFace faceClone = new DCELFace();

            DCELHalfEdge startEdge = OuterComponent ?? throw new NullReferenceException();
            
            // Clone the start edge, so we can link off of that
            DCELHalfEdge headEdgesClone = startEdge.Clone();
            headEdgesClone.SetIncidentFace(faceClone);

            faceClone.SetOuterComponent(headEdgesClone);

            // Keep track of the tail of the tail of the edge chain
            DCELHalfEdge tailEdgesClone = headEdgesClone;
            
            // Because we did the first one seperate, start with the next edge
            DCELHalfEdge currentEdge = startEdge.Next ?? throw new NullReferenceException();

            do
            {
                // Make a clone of the current edge
                DCELHalfEdge edgeClone = currentEdge.Clone();
                edgeClone.SetIncidentFace(faceClone);
                edgeClone.Twin?.SetNextAndItsPrev(tailEdgesClone.Twin ?? throw new NullReferenceException());

                // Chain it to the tail of the edge chain and set as tail
                tailEdgesClone.SetNextAndItsPrev(edgeClone);

                tailEdgesClone = edgeClone;

                currentEdge = currentEdge.Next ?? throw new NullReferenceException();
            }
            while (currentEdge != startEdge);

            // Tie the head and tail together
            tailEdgesClone.SetNextAndItsPrev(headEdgesClone);
            headEdgesClone.Twin?.SetNextAndItsPrev(tailEdgesClone.Twin ?? throw new NullReferenceException());

            return faceClone;
        }

        public IEnumerator<DCELHalfEdge> GetEnumerator()
        {
            return new EdgeEnumerator(OuterComponent ?? throw new NullReferenceException());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public bool Equals(DCELFace? other)
        {
            if (other == null) return false;

            if (this.Count() != other.Count()) return false;

            foreach ((DCELHalfEdge edge, DCELHalfEdge otherEdge) in Enumerable.Zip<DCELHalfEdge, DCELHalfEdge>(this, other))
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

            return this.Equals(obj as DCELFace);
        }

        public override int GetHashCode()
        {
            return (this.Select(edge => edge.GetHashCode())).GetHashCode();
        }
    }

    public class FaceEnumerable : IEnumerable<DCELHalfEdge>
    {
        DCELFace face;

        public FaceEnumerable(DCELFace face)
        {
            this.face = face;
        }

        public IEnumerator<DCELHalfEdge> GetEnumerator()
        {
            return new EdgeEnumerator(face.OuterComponent ?? throw new NullReferenceException());
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public class EdgeEnumerator : IEnumerator<DCELHalfEdge>
    {
        private DCELHalfEdge currentEdge, startEdge;

        public DCELHalfEdge Current => currentEdge;

        object IEnumerator.Current => currentEdge;

        public EdgeEnumerator(DCELHalfEdge startEdge)
        {
            this.startEdge = startEdge;
            currentEdge = startEdge;
        }

        public bool MoveNext()
        {
            currentEdge = currentEdge.Next ?? throw new NullReferenceException();

            return currentEdge.ID != startEdge.ID;
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