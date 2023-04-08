namespace LSPainter.DCEL
{
    public class DCELFace
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

        public DCELHalfEdge? OuterComponent { get; set; }
        
        public List<DCELHalfEdge> InnerComponents;

        public DCELFace()
        {
            InnerComponents = new List<DCELHalfEdge>();
        }

        public void AddInnerComponent(DCELHalfEdge innerComponent)
        {
            InnerComponents.Add(innerComponent);
        }

        public void SetOuterComponent(DCELHalfEdge outerComponent)
        {
            OuterComponent = outerComponent;
        }

        public override string ToString()
        {
            return $"Face {ID}";
        }
    }
}