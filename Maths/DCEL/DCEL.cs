namespace LSPainter.Maths.DCEL
{
    public class DCEL
    {
        public IEnumerable<Vertex> Vertices => vertices.Values;
        public IEnumerable<HalfEdge> HalfEdges => halfEdges.Values;
        public IEnumerable<Face> Faces => faces.Values;

        private Dictionary<uint, Vertex> vertices;
        private Dictionary<uint, HalfEdge> halfEdges;
        private Dictionary<uint, Face> faces;

        public DCEL()
        {
            vertices = new Dictionary<uint, Vertex>();
            halfEdges = new Dictionary<uint, HalfEdge>();
            faces = new Dictionary<uint, Face>();
        }

        public void AddVertices(IEnumerable<Vertex> vertices)
        {
            foreach (Vertex vertex in vertices)
            {
                AddVertex(vertex);
            }
        }

        public void AddVertex(Vertex vertex)
        {
            vertices.Add(vertex.ID, vertex);
        }

        public void AddHalfEdges(IEnumerable<HalfEdge> halfEdges)
        {
            foreach (HalfEdge halfEdge in halfEdges)
            {
                AddHalfEdge(halfEdge);
            }
        }

        public void AddHalfEdge(HalfEdge halfEdge)
        {
            halfEdges.Add(halfEdge.ID, halfEdge);
        }

        public void AddFaces(IEnumerable<Face> faces)
        {
            foreach (Face face in faces)
            {
                AddFace(face);
            }
        }

        public void AddFace(Face face)
        {
            faces.Add(face.ID, face);
        }

        /// <summary>
        /// Add an edge between v1 and v2, and set all references correctly.
        /// Note: edge (v1, v2) is a diagonal of their shared incident face.
        /// </summary>
        /// <returns>
        /// A tuple with the two faces that have been created by adding the
        /// edge. The first face is new, the second face is the old face. 
        /// </returns>
        public static (Face, Face) AddEdge(Vertex v1, Vertex v2)
        {
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
    }
}