using LSPainter.Geometry;

namespace LSPainter.Geometry
{
    // Doubly connected edge list: https://en.wikipedia.org/wiki/Doubly_connected_edge_list
    public class DCEL
    {
        Dictionary<uint, Vertex> vertices;
        Dictionary<uint, HalfEdge> halfEdges;
        Dictionary<uint, Face> faces;

        Vertex topLeft, topRight, bottomLeft, bottomRight;
        HalfEdge top, topT, bottom, bottomT, left, leftT, right, rightT;
        Face face;

        public DCEL(int width, int height)
        {
            topLeft = new Vertex(0, 0);
            topRight = new Vertex(width, 0);
            bottomLeft = new Vertex(0, height);
            bottomRight = new Vertex(width, height);

            top = new HalfEdge();
            topT = new HalfEdge();
            right = new HalfEdge();
            rightT = new HalfEdge();
            bottom = new HalfEdge();
            bottomT = new HalfEdge();
            left = new HalfEdge();
            leftT = new HalfEdge();

            face = new Face();

            // Set vertices
            // Clockwise rotation
            topLeft.SetIncidentEdge(top);
            topRight.SetIncidentEdge(right);
            bottomRight.SetIncidentEdge(bottom);
            bottomLeft.SetIncidentEdge(left);

            // Set Half-edges, first inwards half-edges
            top.SetIncidentFace(face);
            right.SetIncidentFace(face);
            bottom.SetIncidentFace(face);
            left.SetIncidentFace(face);

            top.SetNextAndItsPrev(right);
            right.SetNextAndItsPrev(bottom);
            bottom.SetNextAndItsPrev(left);
            left.SetNextAndItsPrev(top);

            top.SetOrigin(topLeft);
            right.SetOrigin(topRight);
            bottom.SetOrigin(bottomRight);
            left.SetOrigin(bottomLeft);

            // Set outward half-edges
            topT.SetIncidentFace(face);
            rightT.SetIncidentFace(face);
            bottomT.SetIncidentFace(face);
            leftT.SetIncidentFace(face);

            // Note: counter-clockwise
            topT.SetNextAndItsPrev(leftT);
            rightT.SetNextAndItsPrev(bottomT);
            bottomT.SetNextAndItsPrev(rightT);
            leftT.SetNextAndItsPrev(topT);

            topT.SetOrigin(topLeft);
            rightT.SetOrigin(topRight);
            bottomT.SetOrigin(bottomRight);
            leftT.SetOrigin(bottomLeft);

            // Set twins
            top.SetTwinAndItsTwin(topT);
            right.SetTwinAndItsTwin(rightT);
            bottom.SetTwinAndItsTwin(bottomT);
            left.SetTwinAndItsTwin(leftT);

            // Set face
            face.SetOuterComponent(top);

            vertices = new Dictionary<uint, Vertex>
            {
                { topLeft.ID, topLeft  },
                { topRight.ID, topRight  },
                { bottomLeft.ID, bottomLeft  },
                { bottomRight.ID, bottomRight  },
            };

            halfEdges = new Dictionary<uint, HalfEdge>
            {
                { top.ID, top },
                { topT.ID, topT },
                { right.ID, right },
                { rightT.ID, rightT },
                { bottom.ID, bottom },
                { bottomT.ID, bottomT },
                { left.ID, left },
                { leftT.ID, leftT },
            };

            faces = new Dictionary<uint, Face>
            {
                { face.ID, face }
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
            faces.Add(face.ID, face);
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

            float pi = (float)Math.PI;

            // There is already an incident edge. Determine the order of next/prev edges
            float angle = GetEdgeAngleWithXAxis(e);

            HalfEdge? current = v.IncidentEdge;
            float angleCurrent = GetEdgeAngleWithXAxis(current);
            float angleNext;

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
            e.SetNextAndItsPrev(current?.Next ?? throw new NullReferenceException());
            e.SetPrevAndItsNext(current?.Twin ?? throw new NullReferenceException());
        }

        bool AngleInRange(float angle, float lb, float ub)
        {
            // Normal case: 0 rad is not in the angle range
            if (lb < ub) return lb <= angle && angle <= ub;

            // Edge case: 0 rad is in the range
            return lb <= angle || angle <= ub;
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