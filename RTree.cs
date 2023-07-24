using LSPainter.Maths;

namespace LSPainter
{
    public class RTree<T> where T : IBoundable
    {
        private class Node
        {
            // List to store objects in this node
            public List<T> Objects { get; }

            // List to store child nodes in case this is not a leaf node
            public List<Node> Children { get; }

            // Bounding box of this node
            public Rectangle BoundingBox { get; set; }

            public Node()
            {
                Objects = new List<T>();
                Children = new List<Node>();

                BoundingBox = Rectangle.Empty;
            }

            Rectangle CreateBoundingBox()
            {
                Rectangle output = Rectangle.Empty;

                foreach (T obj in Objects)
                {
                    output.UnionWith(obj.BoundingBox);
                }

                foreach (Node child in Children)
                {
                    output.UnionWith(child.BoundingBox);
                }

                return output;
            }
        }

        private Node root;

        private int maxEntriesPerNode;

        public RTree(int maxEntriesPerNode)
        {
            this.maxEntriesPerNode = maxEntriesPerNode;

            root = new Node();
        }

        public void Insert(T obj)
        {

        }
    }
}