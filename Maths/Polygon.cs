using LSPainter;
using LSPainter.Shapes;
using LSPainter.DCEL;

namespace LSPainter.Maths
{
    class HalfEdgeComparer : IComparer<DCELHalfEdge?>
    {
        LineSegmentComparer comparer = new LineSegmentComparer();

        public int Compare(DCELHalfEdge? l, DCELHalfEdge? m)
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

    public class Polygon
    {
        DCELFace dcel, triangulation;
        DCELVertex[] vertices;

        public Polygon(DCELFace face)
        {
            dcel = face;
            triangulation = face.Clone();

            List<DCELVertex> vertices = new List<DCELVertex>();

            foreach (DCELHalfEdge halfEdge in dcel)
            {
                vertices.Add(halfEdge.Origin ?? throw new NullReferenceException());
            }

            this.vertices = vertices.ToArray();

            Triangulate();
        }

        public static Polygon FromFace(DCELFace face)
        {
            return new Polygon(face.Clone());
        }

        void Triangulate()
        {

        }

        void LineSweep()
        {
            // https://www.cs.uu.nl/docs/vakken/ga/2022/slides/slides3.pdf
            Queue<DCELVertex> eventQueue = new Queue<DCELVertex>(vertices.OrderBy(v => v.Y));

            HalfEdgeComparer comparer = new HalfEdgeComparer();
            SortedList<DCELHalfEdge, DCELVertex> status = new SortedList<DCELHalfEdge, DCELVertex>(comparer);

            HashSet<DCELVertex> checkedVertices = new HashSet<DCELVertex>();

            while (eventQueue.Count != 0)
            {
                /* 
                Find what type of vertex this is by looking if the next and prev vertices have
                been seen or not. Because the polygon is a face, the next vertex will always
                be the CW next vertex. This combination makes the type checking very easy.
                 */
                DCELVertex vertex = eventQueue.Dequeue();

                DCELHalfEdge nextEdge = vertex.IncidentEdge ?? throw new NullReferenceException();
                DCELHalfEdge prevEdge = nextEdge.Prev ?? throw new NullReferenceException();

                DCELVertex nextVertex = nextEdge.Next?.Origin ?? throw new NullReferenceException();
                DCELVertex prevVertex = prevEdge.Origin ?? throw new NullReferenceException();

                bool nextVertexChecked = checkedVertices.Contains(nextVertex);
                bool prevVertexChecked = checkedVertices.Contains(prevVertex);

                if (!nextVertexChecked && !prevVertexChecked)
                {
                    /* 
                    Start vertex: insert the CCW incident edge in the status with this vertex as helper.
                    Since the face lies to the right of each half-edge, we know that v.IncidentEdge.prev
                    is the CCW incident edge.
                     */

                    status.Add(prevEdge, vertex);
                }
                else if (nextVertexChecked && prevVertexChecked)
                {
                    // Merge or end vertex. Coincidentally, remove nextEdge in both cases
                    status.Remove(nextEdge);

                    if (comparer.Compare(nextEdge, prevEdge) > 0)
                    {
                        /* 
                        The next edge is right of the prev edge. This means it's a merge vector.
                        Set this vertex as new helper of the first edge to the left of the vertex.
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
        }

        static void UpdateHelpers(SortedList<DCELHalfEdge, DCELVertex> status, DCELVertex newHelper)
        {
            (DCELHalfEdge halfEdge, DCELVertex oldHelper) = status.MinBy((kvp) =>
            {
                (DCELHalfEdge halfEdge, DCELVertex helper) = kvp;
                LineSegment edge = (LineSegment)halfEdge;

                double xEdge = edge.GetXFromY(newHelper.Y);

                double diff = newHelper.X - xEdge;

                if (diff < 0)
                {
                    diff = double.MaxValue;
                }

                return diff;
            });

            status[halfEdge] = newHelper;
        }
    }
}