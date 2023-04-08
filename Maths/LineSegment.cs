namespace LSPainter.Maths
{
    public struct LineSegment
    {
        public Vector V1, V2;

        public LineSegment(Vector v1, Vector v2)
        {
            V1 = v1;
            V2 = v2;
        }

        public static LineSegment operator -(LineSegment l) => new LineSegment(l.V2, l.V1);

        public double GetXFromY(double y)
        {
            if (V2.Y - V1.Y == 0)
            {
                // Line segment is horizontal, just use an x-coord of either endpoint
                return V1.X;
            }

            double t = (y - V1.Y) / (V2.Y - V1.Y);

            if (0 <= t && t <= 1)
            {
                throw new Exception("not on line segment");
            }

            double x = V1.X + t * (V2.X - V1.X);

            return x;
        }
    }
}