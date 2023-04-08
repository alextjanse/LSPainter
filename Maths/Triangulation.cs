using LSPainter;
using LSPainter.Shapes;
using LSPainter.DCEL;

namespace LSPainter.Maths
{
    public class Triangulation : IComparer<LineSegment>
    {
        public static Triangulation FromFace(DCELFace face)
        {
            List<DCELHalfEdge> edges = new List<DCELHalfEdge>();
            List<DCELVertex> vertices = new List<DCELVertex>();

            DCELHalfEdge startEdge = face.OuterComponent ?? throw new NullReferenceException();
            DCELHalfEdge currentEdge = startEdge;
            DCELVertex vertex;

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
            Queue<DCELVertex> eventQueue = new Queue<DCELVertex>(vertices.OrderBy(v => v.Y));
            SortedList<DCELHalfEdge, DCELVertex> status = new SortedList<DCELHalfEdge, DCELVertex>();

            HashSet<DCELHalfEdge> checkedEdges = new HashSet<DCELHalfEdge>();

            double height = 0;

            while (eventQueue.Count != 0)
            {
                vertex = eventQueue.Dequeue();
                DCELHalfEdge next = vertex.IncidentEdge ?? throw new NullReferenceException();
                DCELHalfEdge prev = next.Prev ?? throw new NullReferenceException();

                bool prevChecked = checkedEdges.Contains(prev);
                bool nextChecked = checkedEdges.Contains(next);

                if (!prevChecked && !nextChecked)
                {
                    // Start vertex: insert incident edges CCW
                        
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

            
            For 
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