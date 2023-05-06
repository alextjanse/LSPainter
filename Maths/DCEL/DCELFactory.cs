using LSPainter.Maths.Shapes;

namespace LSPainter.Maths.DCEL
{
    public class DCELGenerator
    {
        public static DCEL CreateCanvas(int width, int height)
        {
            DCEL dcel = new DCEL();

            Vertex v1 = new Vertex(0, 0);
            Vertex v2 = new Vertex(width, 0);
            Vertex v3 = new Vertex(width, height);
            Vertex v4 = new Vertex(0, height);

            HalfEdge e1 = new HalfEdge();
            HalfEdge e2 = new HalfEdge();
            HalfEdge e3 = new HalfEdge();
            HalfEdge e4 = new HalfEdge();
            HalfEdge e5 = new HalfEdge();
            HalfEdge e6 = new HalfEdge();
            HalfEdge e7 = new HalfEdge();
            HalfEdge e8 = new HalfEdge();

            Face f = new Face();

            // Set vertices
            // Clockwise rotation
            v1.SetIncidentEdge(e1);
            v2.SetIncidentEdge(e2);
            v3.SetIncidentEdge(e3);
            v4.SetIncidentEdge(e4);

            // Set the origins
            e1.SetOrigin(v1);
            e2.SetOrigin(v2);
            e3.SetOrigin(v3);
            e4.SetOrigin(v4);

            // Set incident face of inner cycle
            e1.SetIncidentFace(f);
            e2.SetIncidentFace(f);
            e3.SetIncidentFace(f);
            e4.SetIncidentFace(f);

            // Set twins
            e1.SetTwinAndItsTwin(e5);
            e2.SetTwinAndItsTwin(e6);
            e3.SetTwinAndItsTwin(e7);
            e4.SetTwinAndItsTwin(e8);

            // Make the cycle
            e1.AppendHalfEdge(e2);
            e2.AppendHalfEdge(e3);
            e3.AppendHalfEdge(e4);
            e4.AppendHalfEdge(e1);

            // Set face
            f.SetOuterComponent(e1);

            dcel.AddVertices(new Vertex[] { v1, v2, v3, v4 });
            dcel.AddHalfEdges(new HalfEdge[] { e1, e2, e3, e4, e5, e6, e7, e8 });
            dcel.AddFace(f);

            return dcel;
        }
    }
}