using LSPainter;
using LSPainter.Shapes;

namespace LSPainter.Geometry
{
    public class Triangulation : IComparer<HalfEdge>, IComparer<LineSegment>
    {
        public static Triangulation FromFace(Face face)
        {
            List<HalfEdge> edges = new List<HalfEdge>();
            List<Vertex> vertices = new List<Vertex>();

            HalfEdge startEdge = face.OuterComponent ?? throw new NullReferenceException();
            HalfEdge currentEdge = startEdge;
            Vertex vertex;

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
            Queue<Vertex> eventQueue = new Queue<Vertex>(vertices.OrderBy(v => v.Y));
            SortedList<HalfEdge, Vertex> status = new SortedList<HalfEdge, Vertex>();

            float height = 0;

            while (eventQueue.Count != 0)
            {
                vertex = eventQueue.Dequeue();

                HalfEdge edge = vertex.IncidentEdge ?? throw new NullReferenceException();

                // Height hasn't been updated yet
                bool prevChecked = (edge.Prev?.Origin?.Y < height) || (edge.Prev?.Origin?.Y == height && edge.Prev?.Origin?.X < vertex.X);
                bool nextChecked = (edge.Next?.Origin?.Y < height) || (edge.Next?.Origin?.Y == height && edge.Next?.Origin?.X < vertex.X);

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

        public int Compare(HalfEdge? l, HalfEdge? m)
        {
            // Transform the half-edges into line-segments, and compare those
            Vector l1 = (Vector)(l?.Origin ?? throw new NullReferenceException());
            Vector l2 = (Vector)(l?.Next?.Origin ?? throw new NullReferenceException());

            Vector m1 = (Vector)(m?.Origin ?? throw new NullReferenceException());
            Vector m2 = (Vector)(m?.Next?.Origin ?? throw new NullReferenceException());

            return Compare(new LineSegment(l1, l2), new LineSegment(m1, m2));
        }

        public int Compare(LineSegment l, LineSegment m)
        {
            /* 
            Given two non-intersecting line segments l: (p, p + r) and m: (q, q + s),
            determine which line segment lies first in order. We can use this
            to order the half-edges from left to right in our status.

            The comparison is called when the line-segments share a same height,
            so we know that their y-ranges overlap. From there, we need to
            figure out which line-segment is left of the other.

            I'll be following the following solution to find the intersection point:
            https://stackoverflow.com/a/565282
             */

            // Deconstruct line segments for cleaner code
            Vector l1 = l.V1;
            Vector l2 = l.V2;
            Vector m1 = m.V1;
            Vector m2 = m.V2;

            Vector p = l1;
            Vector r = l2 - l1;
            Vector q = m1;
            Vector s = m2 - m1;

            // Function defined in source
            Func<Vector, Vector, float> Cross = (v, w) => v.X * w.Y - v.Y * w.X;

            if (Cross(r, s) == 0)
            {
                // Case 1-2: lines are "parralel" (def: they don't intersect)

                if (Cross(q - p, r) == 0)
                {
                    // Case 1: lines are "collinear" (def: they lay on top of each other)
                    throw new Exception("line segments are collinear");
                }

                /* 
                Case 2: line segments are not collinear, so l lies in its total to a side of m.
                Do this by checking if l1 lies left of m.

                First, make sure that line segment m is bottom-to-top orientation
                 */
                
                if (m1.Y < m2.Y)
                {
                    // m1 is bottom endpoint
                    return p.CompareTo(m);
                }
                else
                {
                    // m1 is top endpoint, flip m.
                    return p.CompareTo(-m);
                }
            }
            else
            {
                // Case 3-4: lines intersect. Check if segments intersect as well.
                float t = Cross(q - p, s) / Cross(r, s);
                float u = Cross(q - p, r) / Cross(r, s);

                if ((0 <= t && t <= 1) && (0 <= u && u <= 1))
                {
                    if ((t == 0 || t == 1) && (u == 0 || u == 1))
                    {
                        // The line segments intersect at the endpoints. Then 
                    }

                    // Case 3: line segments intersect
                    throw new Exception("line segments intersect");
                }
                else
                {
                    /* 
                    Case 4: Line segments don't intersect. Now, this means that they share a y-range,
                    otherwise the comparison wouldn't have been called. This means that in in their
                    intersecting y-range [y0, y1], one line segment lies in total left of the other.
                     */

                    float y = Math.Max(Math.Min(l1.Y, l2.Y), Math.Min(m1.Y, m2.Y));

                    float lX = l.GetXFromY(y);
                    float mX = m.GetXFromY(y);

                    if (lX < mX)
                    {
                        return -1;
                    }
                    else if (lX > mX)
                    {
                        return 1;
                    }
                    
                    /* 
                    lX = mX, meaning that they share an endpoint at (lX, y0). This means that at
                    y1 the x-coordinates have to have a different value, otherwise the line segments
                    are collinear.
                    */

                    y = Math.Min(Math.Max(l1.Y, l2.Y), Math.Max(m1.Y, m2.Y));

                    lX = l.GetXFromY(y);
                    mX = m.GetXFromY(y);

                    if (lX < mX)
                    {
                        return -1;
                    }
                    else if (lX > mX)
                    {
                        return 1;
                    }

                    throw new Exception("This code should never be reached");
                }
            }
        }
    }
}