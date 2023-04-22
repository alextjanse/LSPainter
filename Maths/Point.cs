namespace LSPainter.Maths
{
    public struct Point : IComparable<LineSegment>
    {
        public double X { get; }
        public double Y { get; }

        public Point(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Vector(Point p) => new Vector(p.X, p.Y);

        public int CompareTo(LineSegment l)
        {
            /* 
            Check if p lies left or right of line segment l, if we look from l1 to l2. Source:
            https://www.scratchapixel.com/lessons/3d-basic-rendering/rasterization-practical-implementation/rasterization-stage.html
             */

            double f = Vector.Determinant(l.V1, l.V2, this);

            if (f < 0)
            {
                return -1;
            }
            else if (f == 0)
            {
                return 0;
            }
            else
            {
                // f > 0
                return 1;
            }
        }
    }
}