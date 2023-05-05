using LSPainter.Maths;
using LSPainter.Maths.Shapes;
using LSPainter.Maths.DCEL;
using LSPainter.LSSolver.Painter;

namespace LSPainter.PlanarSubdivision
{
    // Doubly connected edge list: https://en.wikipedia.org/wiki/Doubly_connected_edge_list
    public class PlanarSubdivisionSolution : CanvasSolution
    {
        Dictionary<uint, Vertex> vertices;
        Dictionary<uint, HalfEdge> halfEdges;
        Dictionary<uint, (Face, Color, Triangulation)> faces;

        Vertex v4, v3, v1, v2;
        HalfEdge e3, e7, e1, e5, e4, e8, e2, e6;
        Face f;

        public PlanarSubdivisionSolution(int width, int height, CanvasComparer comparer) : base(width, height, comparer)
        {
            v1 = new Vertex(0, 0);
            v2 = new Vertex(width, 0);
            v3 = new Vertex(width, height);
            v4 = new Vertex(0, height);

            e1 = new HalfEdge();
            e2 = new HalfEdge();
            e3 = new HalfEdge();
            e4 = new HalfEdge();
            e5 = new HalfEdge();
            e6 = new HalfEdge();
            e7 = new HalfEdge();
            e8 = new HalfEdge();

            f = new Face();

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

            vertices = new Dictionary<uint, Vertex>
            {
                { v1.ID, v1  },
                { v2.ID, v2  },
                { v3.ID, v3  },
                { v4.ID, v4  },
            };

            halfEdges = new Dictionary<uint, HalfEdge>
            {
                { e1.ID, e1 },
                { e2.ID, e2 },
                { e3.ID, e3 },
                { e4.ID, e4 },
                { e5.ID, e5 },
                { e6.ID, e6 },
                { e7.ID, e7 },
                { e8.ID, e8 },
            };


            Triangulation triangulation = new Triangulation(f);

            faces = new Dictionary<uint, (Face, Color, Triangulation)>
            {
                { f.ID, (f, Color.Black, triangulation) }
            };
        }

        void AddVertex(Vertex vertex)
        {
            vertices.Add(vertex.ID, vertex);
        }

        void AddHalfEdge(HalfEdge halfEdge)
        {
            halfEdges.Add(halfEdge.ID, halfEdge);
        }

        void AddFace(Face face)
        {
            faces.Add(face.ID, (face, Color.Black, new Triangulation(face)));
        }

        void AddVertex(double x, double y)
        {
            Vertex vertex = new Vertex(x, y);
            vertices.Add(vertex.ID, vertex);
        }

        public void Draw()
        {
            foreach ((Face face, _, _) in faces.Values)
            {
                DrawFace(face);
            }
        }

        void DrawFace(Face face)
        {
            uint id = face.ID;

            (_, Color color, Triangulation triangulation) = faces[id];

            foreach ((_, Triangle triangle) in triangulation.Triangles)
            {
                DrawShape(triangle, color);
            }
        }

        protected override CanvasChange GenerateCanvasChange()
        {
            Func<PlanarSubdivisionChange>[] generators = new Func<PlanarSubdivisionChange>[]
            {
                GenerateFaceColorChange,
            };

            return Randomizer.PickRandomly(generators)();
        }

        PlanarSubdivisionChange GenerateFaceColorChange()
        {
            Face face = Randomizer.PickRandomly(faces.Values.Select(f => f.Item1));
            Color color = ColorGenerator.Generate();

            return new FaceColorChange(face, color);
        }

        protected override long TryChange(CanvasChange change) => TryPSChange((PlanarSubdivisionChange)change);

        protected override void ApplyChange(CanvasChange change) => ApplyPSChange((PlanarSubdivisionChange)change);

        long TryPSChange(PlanarSubdivisionChange change)
        {
            FaceColorChange fcChange = (FaceColorChange)change;

            Color color = fcChange.Color;

            long scoreDiff = 0;

            foreach (Face face in fcChange.GetAlteredFaces())
            {
                Triangulation triangulation = faces[face.ID].Item3;

                foreach ((_, Triangle triangle) in triangulation.Triangles)
                {
                    scoreDiff += TryDrawShape(triangle, color);
                }
            }

            return scoreDiff;
        }

        void ApplyPSChange(PlanarSubdivisionChange change)
        {
            FaceColorChange fcChange = (FaceColorChange)change;

            Color color = fcChange.Color;

            foreach (Face face in fcChange.GetAlteredFaces())
            {
                Triangulation triangulation = faces[face.ID].Item3;

                foreach ((_, Triangle triangle) in triangulation.Triangles)
                {
                    DrawShape(triangle, color);
                }
            }
        }
    }
}