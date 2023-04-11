using LSPainter.Maths;

namespace LSPainter.DCEL
{
    public class DCELVertex : IEquatable<DCELVertex>
    {
        public static explicit operator Vector(DCELVertex v) => new Vector(v.X, v.Y);
        public static bool operator ==(DCELVertex? u, DCELVertex? v)
        {
            if (u == null) return v == null;
            return u.Equals(v);
        }

        public static bool operator !=(DCELVertex? u, DCELVertex? v) => !(u == v);

        static uint idGen = 1;
        private uint id = 0;
        public uint ID
        {
            get
            {
                if (id == 0)
                {
                    id = idGen++;
                }
                return id;
            }
        }
        
        public double X { get; }
        public double Y { get; }

        public DCELHalfEdge? IncidentEdge { get; set; }

        public DCELVertex(double x, double y)
        {
            X = x;
            Y = y;
        }

        public void SetIncidentEdge(DCELHalfEdge incidentEdge)
        {
            IncidentEdge = incidentEdge;
        }

        public override string ToString()
        {
            return $"Vertex {ID}";
        }

        public DCELVertex Clone()
        {
            return new DCELVertex(X, Y);
        }

        public bool Equals(DCELVertex? other)
        {
            if (other == null) return false;

            return other.X == X && other.Y == Y;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (ReferenceEquals(obj, null))
            {
                return false;
            }

            return this.Equals(obj as DCELVertex);
        }

        public override int GetHashCode()
        {
            return (X, Y).GetHashCode();
        }
    }
}