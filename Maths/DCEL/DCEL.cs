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
    }
}