using System.Collections;

namespace LSPainter.Maths
{
    public struct Polygon
    {
        public Vertex[] Vertices { get; }
        public Edge[] Edges { get; }

        public Polygon(Vertex[] vertices)
        {
            Vertices = vertices;

            if (Vertices.Length < 3) throw new Exception("Not a polygon");

            Edges = new Edge[Vertices.Length];

            // Add the edges between the vertices
            for (int i = 0; i < Vertices.Length - 1; i++)
            {
                Edges[i] = new Edge(Vertices[i], Vertices[i + 1]);
            }

            // Add the last edge
            Edges[Edges.Length - 1] = new Edge(Vertices[Vertices.Length - 1], Vertices[0]);
        }
    }
}