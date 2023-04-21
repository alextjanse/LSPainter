using LSPainter.Maths;

namespace LSPainter.DCEL
{
    public class Vertex : IEquatable<Vertex>
    {
        public static explicit operator Vector(Vertex v) => new Vector(v.X, v.Y);
        public static bool operator ==(Vertex? u, Vertex? v)
        {
            if (u is null) return v is null;
            return u.Equals(v);
        }

        public static bool operator !=(Vertex? u, Vertex? v) => !(u == v);

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
        
        public double X { get; set; }
        public double Y { get; set; }

        public HalfEdge? IncidentEdge { get; set; }

        public Vertex(double x, double y)
        {
            X = x;
            Y = y;

            id = idGen++;
        }

        public void SetXY(double x, double y)
        {
            X = x;
            Y = y;
        }

        public void SetIncidentEdge(HalfEdge? incidentEdge)
        {
            if (incidentEdge == null) throw new NullReferenceException();
            IncidentEdge = incidentEdge;
        }

        public bool IsSetCorrectly()
        {
            if (IncidentEdge == null) return false;

            if (IncidentEdge.Origin != this) return false;

            return true;
        }

        public override string ToString()
        {
            return $"Vertex {ID}: ({X}, {Y})";
        }

        public Vertex Clone()
        {
            return new Vertex(X, Y);
        }

        public bool Equals(Vertex? other)
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

            return this.Equals(obj as Vertex);
        }

        public override int GetHashCode()
        {
            return (ID, X, Y).GetHashCode();
        }
    }
}