using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using LSPainter.Maths.DCEL;

namespace LSPainter
{
    public static class Program
    {
        private static void Main()
        {
            TestTriangulation();

            string file_path = "./Images/mona_lisa.jpeg";

            ImageHandler image = new ImageHandler("Mona Lisa", file_path);

            WindowLayout windowLayout = new WindowLayout(1, 1, false, 1);

            using (WindowManager window = new WindowManager(windowLayout, image))
            {
                window.Run();
            }
        }

        static void TestTriangulation()
        {
            Vertex v1 = new Vertex(4, 2);
            Vertex v2 = new Vertex(6, 0);
            Vertex v3 = new Vertex(8, 4);
            Vertex v4 = new Vertex(6, 8);
            Vertex v5 = new Vertex(4, 6);
            Vertex v6 = new Vertex(2, 8);
            Vertex v7 = new Vertex(0, 4);
            Vertex v8 = new Vertex(2, 0);

            HalfEdge e1 = new HalfEdge();
            HalfEdge e2 = new HalfEdge();
            HalfEdge e3 = new HalfEdge();
            HalfEdge e4 = new HalfEdge();
            HalfEdge e5 = new HalfEdge();
            HalfEdge e6 = new HalfEdge();
            HalfEdge e7 = new HalfEdge();
            HalfEdge e8 = new HalfEdge();
            HalfEdge e9 = new HalfEdge();
            HalfEdge e10 = new HalfEdge();
            HalfEdge e11 = new HalfEdge();
            HalfEdge e12 = new HalfEdge();
            HalfEdge e13 = new HalfEdge();
            HalfEdge e14 = new HalfEdge();
            HalfEdge e15 = new HalfEdge();
            HalfEdge e16 = new HalfEdge();

            Face f = new Face();

            v1.SetIncidentEdge(e1);
            v2.SetIncidentEdge(e2);
            v3.SetIncidentEdge(e3);
            v4.SetIncidentEdge(e4);
            v5.SetIncidentEdge(e5);
            v6.SetIncidentEdge(e6);
            v7.SetIncidentEdge(e7);
            v8.SetIncidentEdge(e8);

            e1.SetOrigin(v1);
            e2.SetOrigin(v2);
            e3.SetOrigin(v3);
            e4.SetOrigin(v4);
            e5.SetOrigin(v5);
            e6.SetOrigin(v6);
            e7.SetOrigin(v7);
            e8.SetOrigin(v8);

            e1.SetIncidentFace(f);
            e2.SetIncidentFace(f);
            e3.SetIncidentFace(f);
            e4.SetIncidentFace(f);
            e5.SetIncidentFace(f);
            e6.SetIncidentFace(f);
            e7.SetIncidentFace(f);
            e8.SetIncidentFace(f);

            e1.SetTwinAndItsTwin(e9);
            e2.SetTwinAndItsTwin(e10);
            e3.SetTwinAndItsTwin(e11);
            e4.SetTwinAndItsTwin(e12);
            e5.SetTwinAndItsTwin(e13);
            e6.SetTwinAndItsTwin(e14);
            e7.SetTwinAndItsTwin(e15);
            e8.SetTwinAndItsTwin(e16);

            e1.AppendHalfEdge(e2);
            e2.AppendHalfEdge(e3);
            e3.AppendHalfEdge(e4);
            e4.AppendHalfEdge(e5);
            e5.AppendHalfEdge(e6);
            e6.AppendHalfEdge(e7);
            e7.AppendHalfEdge(e8);
            e8.AppendHalfEdge(e1);

            f.SetOuterComponent(e1);

            Triangulation triangulation = new Triangulation(f);
        }
    }
}
