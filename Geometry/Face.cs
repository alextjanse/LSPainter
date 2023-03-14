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

        private HalfEdge? _outerComponent;
        public HalfEdge OuterComponent
        {
            get
            {
                if (_outerComponent != null)
                {
                    return _outerComponent;
                }
                throw new NullReferenceException($"Outer component of Face ${id} is null");
            }
            set
            {
                _outerComponent = value;
            }
        }
        
        public List<HalfEdge> InnerComponents;

        public Face()
        {
            InnerComponents = new List<HalfEdge>();
        }

        public void AddInnerComponent(HalfEdge innerComponent)
        {
            InnerComponents.Add(innerComponent);
        }
    }
}