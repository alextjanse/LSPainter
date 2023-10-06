namespace LSPainter.Maths
{
    public class Edge : IEquatable<Edge>, IBoundable
    {
        public Vector V1, V2;

        public Rectangle BoundingBox => Rectangle.FromPointCloud(new[] { V1, V2 });

        public Edge(Vector v1, Vector v2)
        {
            V1 = v1;
            V2 = v2;
        }

        public static bool operator ==(Edge? e1, Edge? e2)
        {
            if (e1 is null) return e2 is null;

            return e1.Equals(e2);
        }

        public static bool operator !=(Edge? e1, Edge? e2) => !(e1 == e2);

        public override bool Equals(object? obj) => this.Equals(obj as Edge ?? throw new InvalidCastException());

        public bool Equals(Edge? other)
        {
            if (other is null) return false;

            return (V1 == other.V1 && V2 == other.V2) || (V1 == other.V2 && V2 == other.V1);
        }

        public override int GetHashCode()
        {
            return (V1.X, V1.Y, V2.X, V2.Y).GetHashCode();
        }
    }
}