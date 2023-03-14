using LSPainter.Geometry;

namespace LSPainter.Geometry
{
    public class PlanarSubdivision
    {
        Dictionary<uint, Vertex> vertices;
        Dictionary<uint, HalfEdge> halfEdges;
        Dictionary<uint, Face> faces;

        public PlanarSubdivision(int width, int height)
        {
            vertices = new Dictionary<uint, Vertex>();
            halfEdges = new Dictionary<uint, HalfEdge>();
            faces = new Dictionary<uint, Face>();
        }

        public void AddVertex(float x, float y)
        {
            
        }

        public void AddEdge(Vertex u, Vertex v)
        {
            HalfEdge edge = new HalfEdge();

            edge.Origin = u;
            
            
        }
    }
}