using LSPainter.Geometry;

namespace LSPainter.Geometry
{
    // Doubly connected edge list: https://en.wikipedia.org/wiki/Doubly_connected_edge_list
    public class DCEL
    {
        Dictionary<uint, Vertex> vertices;
        Dictionary<uint, HalfEdge> halfEdges;
        Dictionary<uint, Face> faces;

        public DCEL(int width, int height)
        {
            vertices = new Dictionary<uint, Vertex>();
            halfEdges = new Dictionary<uint, HalfEdge>();
            faces = new Dictionary<uint, Face>();
        }

        void AddVertex(float x, float y)
        {
            Vertex vertex = new Vertex(x, y);
            vertices.Add(vertex.ID, vertex);
        }

        void AddEdge(Vertex u, Vertex v)
        {
            HalfEdge edge = new HalfEdge();

            edge.Origin = u;


        }

        void InsertIncidentEdge(Vertex v, HalfEdge e)
        {
            if (v.IncidentEdge == null)
            {
                v.IncidentEdge = e;
                return;
            }

            // There is already an incident edge. Determine the order of next/prev edges
            float angle = GetEdgeAngleWithXAxis(e);

            HalfEdge current = v.IncidentEdge;

            do
            {
                float angleCurrent = GetEdgeAngleWithXAxis(current);

                float angleNext = GetEdgeAngleWithXAxis(current?.Twin?.Next ?? throw new NullReferenceException());

                if ()
            }
        }

        float GetEdgeAngleWithXAxis(HalfEdge e)
        {
            if (e.Origin == null || e.Twin == null || e.Twin.Origin == null)
            {
                throw new NullReferenceException();
            }

            Vertex origin = e.Origin;
            Vertex destination = e.Twin.Origin;

            float length = (float)Math.Sqrt(
                (origin.X - destination.X) * (origin.X - destination.Y) +
                (origin.Y - destination.Y) * (origin.Y - destination.Y)
            );

            if (length == 0)
            {
                throw new DivideByZeroException($"The distance between {origin} and {destination} is zero");
            }

            // cos(a) = adjacent / hypotenuse -> a = acos(adjacent / hyptenuse)
            return (float)Math.Acos((destination.X - origin.X) / length) + ((destination.Y - origin.Y) < 0 ? (float)Math.PI : 0);
        }
    }
}