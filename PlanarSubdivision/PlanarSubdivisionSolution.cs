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
                // DrawShape(triangle, color);
            }
        }

        /* 
        Probably depracated.
         */

        void InsertIncidentEdge(Vertex v, HalfEdge e)
        {
            if (v.IncidentEdge == null)
            {
                v.IncidentEdge = e;
                return;
            }

            // There is already an incident edge. Determine the order of next/prev edges
            double angle = GetEdgeAngleWithXAxis(e);

            HalfEdge? current = v.IncidentEdge;
            double angleCurrent = GetEdgeAngleWithXAxis(current);
            double angleNext;

            do
            {
                angleNext = GetEdgeAngleWithXAxis(current?.Twin?.Next ?? throw new NullReferenceException());

                if (AngleInRange(angle, angleCurrent, angleNext))
                {
                    // We found the angle where we need to insert the new edge
                    break;
                }

                current = current?.Twin?.Next ?? throw new NullReferenceException();
                angleCurrent = angleNext;
            }
            while (current.ID != v.IncidentEdge.ID);

            // Insert the new edge after the current edge
            e.AppendHalfEdge(current?.Next ?? throw new NullReferenceException());
            e.PrependHalfEdge(current?.Twin ?? throw new NullReferenceException());
        }

        bool AngleInRange(double angle, double lb, double ub)
        {
            // Normal case: 0 rad is not in the angle range
            if (lb < ub) return lb <= angle && angle <= ub;

            // Edge case: 0 rad is in the range
            return lb <= angle || angle <= ub;
        }

        double GetEdgeAngleWithXAxis(HalfEdge e)
        {
            if (e.Origin == null || e.Twin == null || e.Twin.Origin == null)
            {
                throw new NullReferenceException();
            }

            Vertex origin = e.Origin;
            Vertex destination = e.Twin.Origin;

            double length = Math.Sqrt(
                (origin.X - destination.X) * (origin.X - destination.Y) +
                (origin.Y - destination.Y) * (origin.Y - destination.Y)
            );

            if (length == 0)
            {
                throw new DivideByZeroException($"The distance between {origin} and {destination} is zero");
            }

            // cos(a) = adjacent / hypotenuse -> a = acos(adjacent / hyptenuse)
            return Math.Acos((destination.X - origin.X) / length) + ((destination.Y - origin.Y) < 0 ? Math.PI : 0);
        }

        protected override CanvasChange GenerateCanvasChange()
        {
            throw new NotImplementedException();
        }

        protected override long TryChange(CanvasChange change)
        {
            throw new NotImplementedException();
        }

        protected override void ApplyChange(CanvasChange change)
        {
            throw new NotImplementedException();
        }
    }
}