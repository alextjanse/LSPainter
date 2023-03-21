using LSPainter.Shapes;

namespace LSPainter.Geometry
{
    public class Triangulation
    {
        public static Triangulation FromFace(Face face)
        {
            List<HalfEdge> edges = new List<HalfEdge>();
            List<Vertex> vertices = new List<Vertex>();

            HalfEdge startEdge = face.OuterComponent ?? throw new NullReferenceException();
            HalfEdge currentEdge = startEdge;
            Vertex vertex;

            // bool isConvex = true;
            int nEdges = 0;

            do
            {
                if (currentEdge.IncidentFace?.ID != face.ID)
                {
                    throw new Exception("e.Next.IncidentFace != this");
                }

                vertex = currentEdge.Origin ?? throw new NullReferenceException();

                /* 
                Everything is much easier if we just set the incident edge to the current edge.
                That way, when we want to find the previous and next vertices in the polygon,
                we can just use the incident edge from the vertex and traverse from there.
                Also, it's arbitrary which edge is the incident edge of the vertex, so we can
                just change it around without any problems.
                 */
                vertex.SetIncidentEdge(currentEdge);

                edges.Add(currentEdge);
                vertices.Add(vertex);

                nEdges++;
                currentEdge = currentEdge.Next ?? throw new NullReferenceException();
            }
            while (currentEdge.ID != startEdge.ID);

            // https://www.cs.uu.nl/docs/vakken/ga/2022/slides/slides3.pdf
            Queue<Vertex> eventQueue = new Queue<Vertex>(vertices.OrderBy(v => v.Y));
            List<(Vertex, HalfEdge)> status = new List<(Vertex, HalfEdge)>();

            float height = 0;

            while (eventQueue.Count != 0)
            {
                vertex = eventQueue.Dequeue();

                HalfEdge edge = vertex.IncidentEdge ?? throw new NullReferenceException();

                // TODO: check what happens if y-coords are not unique?
                bool prevChecked = edge.Prev?.Origin?.Y < height;
                bool nextChecked = edge.Next?.Origin?.Y < height;

                if (!prevChecked && !nextChecked)
                {
                    // Start vertex
                }
                else if (prevChecked && nextChecked)
                {
                    // End vertex or merge vertex
                }
                else
                {
                    // Regular vertex
                }
            }
        }
    }
}