namespace LSPainter.DCEL
{
    // Doubly connected edge list: https://en.wikipedia.org/wiki/Doubly_connected_edge_list
    public class DCEL
    {
        Dictionary<uint, DCELVertex> vertices;
        Dictionary<uint, DCELHalfEdge> halfEdges;
        Dictionary<uint, DCELFace> faces;

        DCELVertex topLeft, topRight, bottomLeft, bottomRight;
        DCELHalfEdge top, topT, bottom, bottomT, left, leftT, right, rightT;
        DCELFace face;

        public DCEL(int width, int height)
        {
            topLeft = new DCELVertex(0, 0);
            topRight = new DCELVertex(width, 0);
            bottomLeft = new DCELVertex(0, height);
            bottomRight = new DCELVertex(width, height);

            top = new DCELHalfEdge();
            topT = new DCELHalfEdge();
            right = new DCELHalfEdge();
            rightT = new DCELHalfEdge();
            bottom = new DCELHalfEdge();
            bottomT = new DCELHalfEdge();
            left = new DCELHalfEdge();
            leftT = new DCELHalfEdge();

            face = new DCELFace();

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

            vertices = new Dictionary<uint, DCELVertex>
            {
                { topLeft.ID, topLeft  },
                { topRight.ID, topRight  },
                { bottomLeft.ID, bottomLeft  },
                { bottomRight.ID, bottomRight  },
            };

            halfEdges = new Dictionary<uint, DCELHalfEdge>
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

            faces = new Dictionary<uint, DCELFace>
            {
                { face.ID, face }
            };
        }

        void AddVertex(DCELVertex vertex)
        {
            vertices.Add(vertex.ID, vertex);
        }

        void AddHalfEdge(DCELHalfEdge halfEdge)
        {
            halfEdges.Add(halfEdge.ID, halfEdge);
        }

        void AddFace(DCELFace face)
        {
            faces.Add(face.ID, face);
        }

        void AddVertex(double x, double y)
        {
            DCELVertex vertex = new DCELVertex(x, y);
            vertices.Add(vertex.ID, vertex);
        }

        void AddEdge(DCELVertex u, DCELVertex v)
        {
            DCELHalfEdge edge = new DCELHalfEdge();

            edge.Origin = u;


        }

        void InsertIncidentEdge(DCELVertex v, DCELHalfEdge e)
        {
            if (v.IncidentEdge == null)
            {
                v.IncidentEdge = e;
                return;
            }

            // There is already an incident edge. Determine the order of next/prev edges
            double angle = GetEdgeAngleWithXAxis(e);

            DCELHalfEdge? current = v.IncidentEdge;
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
            e.SetNextAndItsPrev(current?.Next ?? throw new NullReferenceException());
            e.SetPrevAndItsNext(current?.Twin ?? throw new NullReferenceException());
        }

        bool AngleInRange(double angle, double lb, double ub)
        {
            // Normal case: 0 rad is not in the angle range
            if (lb < ub) return lb <= angle && angle <= ub;

            // Edge case: 0 rad is in the range
            return lb <= angle || angle <= ub;
        }

        double GetEdgeAngleWithXAxis(DCELHalfEdge e)
        {
            if (e.Origin == null || e.Twin == null || e.Twin.Origin == null)
            {
                throw new NullReferenceException();
            }

            DCELVertex origin = e.Origin;
            DCELVertex destination = e.Twin.Origin;

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
    }
}