using LSPainter.Maths;

namespace LSPainter.DCEL
{
    public class Triangulation
    {
        List<Face> triangles;

        /// <summary>
        /// Stores the IDs of the vertices of the actual face. If the face changes,
        /// we need to update the triangulation as well. If a vertex updates, call
        /// Update() to all its incident face triangulations, so they can update
        /// accordingly.
        /// </summary>
        Dictionary<uint, Vertex> vertices;
        
        HalfEdge outsideEdge;

        public Triangulation(Face face)
        {
            triangles = new List<Face>();
            vertices = new Dictionary<uint, Vertex>();

            Face faceClone = face.Clone();

            outsideEdge = faceClone.OuterComponent?.Twin ?? throw new NullReferenceException();

            // Store the IDs of the vertices of the original face
            foreach ((HalfEdge edgeClone, HalfEdge edge) in Enumerable.Zip<HalfEdge, HalfEdge>(faceClone, face))
            {
                Vertex vertexClone = edgeClone.Origin ?? throw new NullReferenceException();

                // Register the vertex as a clone of this vertex
                vertices.Add(edge.Origin?.ID ?? throw new NullReferenceException(), vertexClone);
            }

            Triangulate();
        }

        public void UpdateVertex(Vertex vertex)
        {
            vertices[vertex.ID].SetXY(vertex.X, vertex.Y);

            //TODO: Check if triangulation needs to be updated. For now, just recompute the triangulation.
            
            Triangulate();
        }

        /// <summary>
        /// Compute the triangulation of the face. Resets the old triangulation.
        /// </summary>
        void Triangulate()
        {
            // Reset. Remove all references to the triangles, to be collected by the GC.
            triangles.Clear();
            Face polygon = ResetPolygon();

            // Recompute the triangulation
            List<Face> yMonotones = LineSweep(polygon);

            foreach (Face yMonotone in yMonotones)
            {
                TriangulateYMonotone(yMonotone);
            }
        }

        /// <summary>
        /// Reset the DCEL to a clean polygon, so we can make a new triangulation.
        /// </summary>
        Face ResetPolygon()
        {
            /* 
            Make a new face that will be the polygon face. Loop over the outside half-edges,
            and make a new twin for each one, with the polygon as its incident face. 
            Note: we're going CCW here, since the outside edge is CCW.
             */
            Face polygon = new Face();

            HalfEdge startEdge = outsideEdge;
            HalfEdge currentEdge = startEdge;

            do
            {
                // The new twin the new outer component of the face
                HalfEdge newTwin = new HalfEdge();

                newTwin.SetOrigin(currentEdge.Twin?.Origin);
                newTwin.Origin?.SetIncidentEdge(newTwin);

                newTwin.SetIncidentFace(polygon);
                newTwin.SetTwinAndItsTwin(currentEdge);

                currentEdge.Prev?.Twin?.PrependHalfEdge(newTwin);

                currentEdge = currentEdge.Next ?? throw new NullReferenceException();
            }
            while (startEdge != currentEdge);

            // Tie the ends together
            currentEdge.Twin?.AppendHalfEdge(currentEdge.Prev?.Twin);

            polygon.SetOuterComponent(startEdge.Twin);

            // All the old faces and edges triangulating will be collected by the GC

            return polygon;
        }

        /// <summary>
        /// Do a line sweep of the face in the given direction. Return a list
        /// of the new faces that have been formed by the addition of edges.
        /// This method is called recursively on each part of the sweep in the
        /// other direction, and then combining all results, giving y-monotone
        /// polygons.
        /// </summary>
        /// <param name="face">The polygon that should be swept</param>
        /// <param name="goingDown">Direction of the line sweep</param>
        /// <returns>List of cuts of the polygon</returns>
        List<Face> LineSweep(Face face, bool goingDown = true)
        {
            // Source: https://www.cs.uu.nl/docs/vakken/ga/2022/slides/slides3.pdf
            
            IEnumerable<Vertex> vertices = face.Select(e => e.Origin ?? throw new NullReferenceException());

            Queue<Vertex> eventQueue = new Queue<Vertex>(vertices.OrderBy(v => (goingDown ? 1 : -1) * v.Y));

            StatusComparer comparer = new StatusComparer();
            SortedList<HalfEdge, Vertex> status = new SortedList<HalfEdge, Vertex>(comparer);

            HashSet<Vertex> checkedVertices = new HashSet<Vertex>();

            /* 
            Let's not distort the DCEL during the line sweep, but keep a list of the edges
            that should be added, and do it after all the vertices have been checked.
             */
            List<(Vertex, Vertex)> newEdges = new List<(Vertex, Vertex)>();
            
            while (eventQueue.Count != 0)
            {
                /* 
                Find what type of vertex this is by looking if the next and prev vertices have
                been passed by the sweep line or not.Because the polygon is a face, the next
                vertex will always be the CW next vertex. This combination makes it very easy
                to check the type of the vertex.

                This technique works for both directions, because we check if the vertex has
                been checked or not. One thing though, the vertex types should be inverted as
                well:
                                            start <-> end
                                            merge <-> split
                The only thing that changes is the name, so I will call the vertex types as if
                we are sweeping top to bottom.
                 */
                Vertex vertex = eventQueue.Dequeue();

                HalfEdge nextEdge = vertex.IncidentEdge ?? throw new NullReferenceException();
                HalfEdge prevEdge = nextEdge.Prev ?? throw new NullReferenceException();

                Vertex nextVertex = nextEdge.Next?.Origin ?? throw new NullReferenceException();
                Vertex prevVertex = prevEdge.Origin ?? throw new NullReferenceException();

                bool nextVertexChecked = checkedVertices.Contains(nextVertex);
                bool prevVertexChecked = checkedVertices.Contains(prevVertex);

                if (!nextVertexChecked && !prevVertexChecked)
                {
                    /* 
                    Start or split vertex. Depends on the order of next and prev.
                     */
                    if (comparer.Compare(nextEdge, prevEdge) > 0)
                    {
                        // Start vertex. Next is right of prev. Add left edge to status with helper
                        status.Add(prevEdge, vertex);
                    }
                    else
                    {
                        // Split vertex. Add an edge from the split vertex to the helper of the edge left of it
                        (_, Vertex helper) = SearchInStatus(status, vertex);

                        newEdges.Add((vertex, helper));
                    }
                }
                else if (nextVertexChecked && prevVertexChecked)
                {
                    // Merge or end vertex. Remove next from status in both cases
                    status.Remove(nextEdge);

                    if (comparer.Compare(nextEdge, prevEdge) > 0)
                    {
                        /* 
                        Merge vector. Next is right of prev. Set this vertex as new helper
                        of the first edge to the left of the vertex.
                         */
                        UpdateHelpers(status, vertex);
                    }
                }
                else if (nextVertexChecked && !prevVertexChecked)
                {
                    // Regular vertex, left side of polygon.
                    status.Remove(nextEdge);
                    status.Add(prevEdge, vertex);
                }
                else // if (!nextVertexChecked && prevVertexChecked)
                {
                    // Regular vertex, right side of polygon.
                    UpdateHelpers(status, vertex);
                }
            }

            List<Face> splits = new List<Face>();

            foreach ((Vertex u, Vertex v) in newEdges)
            {
                // Use the face to refer to the rest of the polygon
                (Face newFace, face) = AddEdge(u, v);
                splits.Add(newFace);
            }

            // Add what's left of the face
            splits.Add(face);

            if (goingDown)
            {
                // Call a upwards line sweep for each split and add all faces together

                List<Face> output = new List<Face>();

                foreach (Face split in splits)
                {
                    output.AddRange(LineSweep(split, false));
                }

                return output;
            }

            return splits;
        }

        /// <summary>
        /// Add an edge between u and v, and set all references correctly.
        /// </summary>
        /// <returns>
        /// A tuple with the two faces that have been created by adding the
        /// edge. The first face is new, the second face is the old face. 
        /// </returns>
        (Face, Face) AddEdge(Vertex u, Vertex v)
        {
            /* 
            I'm picturing that u is the bottom vertex and v the upper vertex.
            That is, u.Y < v.Y. From there, the half-edge is from u to v, and
            its twin from v to u.
             */

            HalfEdge halfEdge = new HalfEdge();
            HalfEdge twin = new HalfEdge();

            HalfEdge uNext = u.IncidentEdge ?? throw new NullReferenceException();
            HalfEdge uPrev = uNext.Prev ?? throw new NullReferenceException();

            HalfEdge vNext = v.IncidentEdge ?? throw new NullReferenceException();
            HalfEdge vPrev = vNext.Prev ?? throw new NullReferenceException();

            Face leftFace = new Face();
            // We can reuse the current face in the DCEL as the right face
            Face rightFace = uPrev.IncidentFace ?? throw new NullReferenceException();

            halfEdge.SetOrigin(u);
            halfEdge.SetNextAndItsPrev(vNext);
            halfEdge.SetPrevAndItsNext(uPrev);

            twin.SetOrigin(v);
            twin.SetNextAndItsPrev(uNext);
            twin.SetPrevAndItsNext(vPrev);

            halfEdge.SetTwinAndItsTwin(twin);

            halfEdge.SetIncidentFace(rightFace);
            rightFace.SetOuterComponent(halfEdge);

            twin.SetIncidentFace(leftFace);
            leftFace.SetOuterComponent(twin);

            foreach (HalfEdge e in leftFace)
            {
                e.SetIncidentFace(leftFace);
            }

            // We don't need to do it for the rightFace, since that was the old face

            return (leftFace, rightFace);
        }

        /// <summary>
        /// Search in the status for the edge and its helper left of the given vertex.
        /// </summary>
        /// <returns>A tuple with the edge and its helper</returns>
        (HalfEdge, Vertex) SearchInStatus(SortedList<HalfEdge, Vertex> status, Vertex vertex)
        {
            /* 
            There is room for improvement here. Right now it works in O(n) because of MinBy.
            Might have to implement a data structure myself to get O(n log n)... TODO
             */

            // Don't understand why I can't just return immediately
            (HalfEdge edge, Vertex helper) = status.MinBy((kvp) =>
            {
                (HalfEdge halfEdge, _) = kvp;
                LineSegment edge = (LineSegment)halfEdge;

                double xEdge = edge.GetXFromY(vertex.Y);

                double diff = vertex.X - xEdge;

                if (diff < 0)
                {
                    diff = double.MaxValue;
                }

                return diff;
            });

            return (edge, helper);
        }

        /// <summary>
        /// Set the helper of the edge left of the new helper.
        /// </summary>
        void UpdateHelpers(SortedList<HalfEdge, Vertex> status, Vertex newHelper)
        {
            (HalfEdge halfEdge, _) = SearchInStatus(status, newHelper);
            status[halfEdge] = newHelper;
        }

        /// <summary>
        /// The types of vertices in a y-monotone polygon.
        /// </summary>
        enum VertexType { Top, LeftChain, RightChain, Bottom }

        /// <summary>
        /// Triangulate the y-monotone and store the created triangle faces
        /// in the triangle list.
        /// </summary>
        void TriangulateYMonotone(Face yMonotone)
        {
            /* 
            Sources:
            - http://homepages.math.uic.edu/~jan/mcs481/triangulating.pdf
            - https://www.cs.umd.edu/class/spring2020/cmsc754/Lects/lect05-triangulate.pdf
             */

            List<(Vertex, VertexType)> sortedVertices = SortVerticesInYMonotone(yMonotone);

            Stack<(Vertex, VertexType)> stack = new Stack<(Vertex, VertexType)>();
            List<(Vertex, Vertex)> newEdges = new List<(Vertex, Vertex)>();

            stack.Push(sortedVertices[0]);
            stack.Push(sortedVertices[1]);

            for (int i = 2; i < sortedVertices.Count - 1; i++)
            {
                (Vertex vertex, VertexType type) = sortedVertices[i];

                (Vertex topVertex, VertexType topType) = stack.Peek();

                if (type != topType)
                {
                    // Vertices are in opposite chains
                    while (stack.Count > 1)
                    {
                        (Vertex previousVertex, _) = stack.Pop();
                        newEdges.Add((vertex, previousVertex));
                    }

                    stack.Pop();
                    stack.Push(sortedVertices[i - 1]);
                    stack.Push((vertex, type));
                }
                else
                {
                    if (!IsReflexVertex(topVertex)) // This is u_{i - 1}
                    {
                        // u_{i - 1} is a nonreflex vertex. Triangulate as much as possible

                        stack.Pop(); // Throw away u_{i - 1} from the stack

                        (Vertex, VertexType) last = stack.Pop();
                        (Vertex currentVertex, VertexType currentType) = last;

                        while (DiagonalLiesInPolygon(vertex, currentVertex, currentType))
                        {
                            newEdges.Add((vertex, currentVertex));
                            (currentVertex, currentType) = stack.Pop();
                        }

                        stack.Push(last);
                        stack.Push((vertex, type));
                    }
                    else
                    {
                        // Vertex is a reflex vertex. Add it to the stack
                        stack.Push((vertex, type));
                    }
                }
            }

            Vertex bottomVertex = sortedVertices.Last().Item1;

            stack.Pop();
            while (stack.Count > 1)
            {
                (Vertex v, _) = stack.Pop();
                newEdges.Add((v, bottomVertex));
            }

            // Write directly into the class list, saves some concatenations
            foreach ((Vertex u, Vertex v) in newEdges)
            {
                // Use the y-monotone as rest of the polygon
                (Face newTriangle, yMonotone) = AddEdge(u, v);
                triangles.Add(newTriangle);
            }
            
            // What's left of the y-monotone is the last triangle
            triangles.Add(yMonotone);
        }

        /// <summary>
        /// Check whether the vertex belongs to the left chain or not.
        /// </summary>
        /// <returns>True if left chain, false if right chain.</returns>
        bool VertexIsInLeftChain(Vertex vertex)
        {
            Vertex nextVertex = vertex.IncidentEdge?.Next?.Origin ?? throw new NullReferenceException();
            Vertex prevVertex = vertex.IncidentEdge?.Prev?.Origin ?? throw new NullReferenceException();

            return prevVertex.Y <= vertex.Y && vertex.Y <= nextVertex.Y;
        }

        /// <summary>
        /// Check whether the vertex' inner angle is larger than pi or not.
        /// </summary>
        bool IsReflexVertex(Vertex vertex)
        {
            Vector v = (Vector)vertex;
            Vector v1 = (Vector)(vertex.IncidentEdge?.Next?.Origin ?? throw new NullReferenceException());
            Vector v2 = (Vector)(vertex.IncidentEdge?.Prev?.Origin ?? throw new NullReferenceException());
            
            /* 
            A reflex vertex is a vertex whose inner angle (angle in the face) is at least pi.
            To check this, we will check if v2 lies right of the line from v to v1. To picture
            it, imagine that v is the origin and v1 lies somewhere on the x-axis. The angle we
            are looking for is the angle between (v2 - v) and (v1 - v). The angle is at least pi
            if v2.Y < 0. That would mean that it lies right of the line from v to v1.
             */

            LineSegment baseLine = new LineSegment(v, v1);
            return ((Point)v2).CompareTo(baseLine) >= 0; // V2 lies right of the line from v to v1
        }

        /// <summary>
        /// Check whether the diagonal between vertices u and v lies in the polygon or not.
        /// </summary>
        /// <param name="type">The vertex type of v.</param>
        bool DiagonalLiesInPolygon(Vertex u, Vertex v, VertexType type)
        {
            /* 
            Take the down going half-edge that is incident to the previous vertex, and make
            a line out of it. Because the face always lays right of a half-edge, we can check
            if the vertex lies right of the line segment, and we're done!
            */

            LineSegment incidentEdgeGoingDown;

            switch (type)
            {
                case VertexType.LeftChain:
                    incidentEdgeGoingDown = (LineSegment)(v.IncidentEdge?.Prev ?? throw new NullReferenceException());
                    break;

                case VertexType.RightChain:
                    incidentEdgeGoingDown = (LineSegment)(v.IncidentEdge ?? throw new NullReferenceException());
                    break;

                default:
                    throw new Exception("Somehow, the vertex is a top or bottom vertex");
            }

            return ((Point)(Vector)u).CompareTo(incidentEdgeGoingDown) > 0;
        }

        /// <summary>
        /// Sort the vertices in the y-monotone by doing a merge sort of
        /// the left and right chains. 
        /// </summary>
        /// <returns>Sorted list of vertices with their vertex type.</returns>
        List<(Vertex, VertexType)> SortVerticesInYMonotone(Face yMonotone)
        {
            /* 
            We need the vertices sorted from top to bottom for the triangulation.
            We also need the incident half-edge values, so just set them for each
            passing each vertex to the half-edge of the face.
             */
            List<(Vertex, VertexType)> output = new List<(Vertex, VertexType)>();

            // Find the top vertex
            HalfEdge topMost = yMonotone.OuterComponent ?? throw new NullReferenceException();

            while ((topMost.Prev?.Origin?.Y > topMost.Origin?.Y))
            {
                topMost = topMost.Prev ?? throw new NullReferenceException();
            }

            while ((topMost.Next?.Origin?.Y > topMost.Origin?.Y))
            {
                topMost = topMost.Next ?? throw new NullReferenceException();
            }

            // e.Origin is the top-most vertex

            output.Add((topMost.Origin ?? throw new NullReferenceException(), VertexType.Top));

            // Set the incident edges of each vertex to the current loop for ease
            topMost.Origin?.SetIncidentEdge(topMost);

            HalfEdge left = topMost.Prev ?? throw new NullReferenceException();
            HalfEdge right = topMost.Next ?? throw new NullReferenceException();

            do
            {
                left.Origin?.SetIncidentEdge(left);
                right.Origin?.SetIncidentEdge(right);

                if (left.Origin?.Y >= right.Origin?.Y)
                {
                    output.Add((left.Origin ?? throw new NullReferenceException(), VertexType.LeftChain));
                    left = left.Prev ?? throw new NullReferenceException();
                }
                else
                {
                    output.Add((right.Origin ?? throw new NullReferenceException(), VertexType.RightChain));
                    right = right.Next ?? throw new NullReferenceException();
                }
            }
            while (left != right);

            (Vertex bottomVertex, _) = output.Last();
            output[output.Count] = (bottomVertex, VertexType.Bottom);

            return output;
        }
    }

    class StatusComparer : IComparer<HalfEdge?>
    {
        LineSegmentComparer comparer = new LineSegmentComparer();

        public int Compare(HalfEdge? l, HalfEdge? m)
        {
            return comparer.Compare((LineSegment)l, (LineSegment)m);
        }
    }

    class LineSegmentComparer : IComparer<LineSegment>
    {
        public int Compare(LineSegment l, LineSegment m)
        {
            /* 
            Given two non-intersecting line segments l: (l1, l2) and m: (m1, m2),
            determine which line segment lies left of the other. We can use this
            to order the half-edges from left to right in our status.

            The comparison is called when the line-segments share a same height,
            so we know they share a y-range [yMin, yMax]. We can then take 4 points
            of interest: lMin: (l[yMin], yMin) and lMax: (l[yMax], yMax). These points
            should both be on either side of m, otherwise the line segments intersect.
             */
            Vector l1 = l.V1;
            Vector l2 = l.V2;

            Vector m1 = m.V1;
            Vector m2 = m.V2;

            double yMin = Math.Max(Math.Min(l1.Y, l2.Y), Math.Min(m1.Y, m2.Y));
            double yMax = Math.Min(Math.Max(l1.Y, l2.Y), Math.Max(m1.Y, m2.Y));

            if (yMax - yMin == 0)
            {
                // The lines are both horizontal
                if ((l1.X == m1.X && l2.X == m2.X) ||
                    (l1.X == m2.X && l2.X == m1.X))
                {
                    throw new Exception("the line segments are the same");
                }

                if (l1.X <= m1.X && l1.X <= m2.X && l2.X <= m1.X && l2.X <= m2.X) return -1;
                else if (l1.X >= m1.X && l1.X >= m2.X && l2.X >= m1.X && l2.X >= m2.X) return 1;
                else throw new Exception("The line segments overlap");
            }

            double lxMin = l.GetXFromY(yMin);
            double lxMax = l.GetXFromY(yMax);

            double mxMin = m.GetXFromY(yMin);
            double mxMax = m.GetXFromY(yMax);

            Point lMin = new Point(lxMin, yMin);
            Point lMax = new Point(lxMax, yMax);

            bool lMinLeftOfM = lMin.CompareTo(m) == -1;
            bool lMaxLeftOfM = lMax.CompareTo(m) == -1;

            if (lMinLeftOfM && lMaxLeftOfM) return -1;
            else if (!lMinLeftOfM && !lMaxLeftOfM) return 1;
            else throw new Exception("line segments are intersecting");
        }
    }
}