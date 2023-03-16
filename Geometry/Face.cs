namespace LSPainter.Geometry
{
    public class Face
    {
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

        public HalfEdge? OuterComponent { get; set; }
        
        public List<HalfEdge> InnerComponents;

        public Face()
        {
            InnerComponents = new List<HalfEdge>();
        }

        public void AddInnerComponent(HalfEdge innerComponent)
        {
            InnerComponents.Add(innerComponent);
        }

        public override string ToString()
        {
            return $"Face {ID}";
        }
    }
}