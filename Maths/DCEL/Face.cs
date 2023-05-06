using System.Collections;

namespace LSPainter.Maths.DCEL
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

            id = idGen++;
        }

        public void AddInnerComponent(HalfEdge innerComponent)
        {
            InnerComponents.Add(innerComponent);
        }

        public void SetOuterComponent(HalfEdge? outerComponent)
        {
            OuterComponent = outerComponent;
        }

        public bool IsSetCorrectly()
        {
            if (OuterComponent == null) return false;

            foreach (HalfEdge e in this)
            {
                if (!e.IsSetCorrectly()) return false;

                if (e.IncidentFace != this) return false;
            }

            return true;
        }

        public IEnumerable<Vertex> Vertices => this.Select(e => e.Origin ?? throw new NullReferenceException());
        public IEnumerable<HalfEdge> Edges => this;
        public IEnumerable<Face> AdjacentFaces
        {
            get
            {
                // Return all adjacent faces, distinct by ID
                return this.Select(e => e.Twin?.IncidentFace ?? throw new NullReferenceException())
                    .DistinctBy(f => f.ID);
            }
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

        public (Face, Face) SplitAt(Vertex v1, Vertex v2)
        {
            // Clean the face
            foreach (HalfEdge e in this)
            {
                e.SetAsIncidentEdgeOfOrigin();
            }

            // See drawing in notes for names, or make the drawing yourself

            HalfEdge e2 = v1.IncidentEdge ?? throw new NullReferenceException();
            HalfEdge e1 = e2.Prev ?? throw new NullReferenceException();
            
            HalfEdge e4 = v2.IncidentEdge ?? throw new NullReferenceException();
            HalfEdge e3 = e4.Prev ?? throw new NullReferenceException();

            HalfEdge e5 = new HalfEdge();
            HalfEdge e6 = new HalfEdge();

            Face f1 = new Face();
            Face f2 = e1.IncidentFace ?? throw new NullReferenceException();

            e5.SetOrigin(v1);
            e5.SetNextAndItsPrev(e4);
            e5.SetPrevAndItsNext(e1);

            e6.SetOrigin(v2);
            e6.SetNextAndItsPrev(e2);
            e6.SetPrevAndItsNext(e3);

            e5.SetTwinAndItsTwin(e6);

            e5.SetIncidentFace(f1);
            f1.SetOuterComponent(e5);

            e6.SetIncidentFace(f2);
            f2.SetOuterComponent(e6);

            foreach (HalfEdge e in f1)
            {
                e.SetIncidentFace(f1);
            }

            /* 
            Set the newly created half-edge as the incident edge of its origin.
            This way, all the vertices in the remaining polygon have the polygon
            half-edge as their incident edges.
            */
            e6.SetAsIncidentEdgeOfOrigin();

            /* 
            We don't need to do the same for f2, because that was the old face,
            so all half-edges should have it as incident face already.
             */

            return (f1, f2);
        }

        public IEnumerator<HalfEdge> GetEnumerator()
        {
            HalfEdge current = OuterComponent ?? throw new NullReferenceException();

            do
            {
                yield return current;
                current = current.Next ?? throw new NullReferenceException();
            }
            while (current != OuterComponent);
            
            // return new EdgeEnumerator(OuterComponent ?? throw new NullReferenceException());
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
}