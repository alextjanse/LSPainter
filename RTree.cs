using LSPainter.Maths;

namespace LSPainter
{
    public class RTree<T> where T : IBoundable
    {
        private class Node
        {
            // List to store objects in this node
            public T? Object { get; set; }

            // List to store child nodes in case this is not a leaf node
            public List<Node> Children { get; }

            // Bounding box of this node
            public Rectangle BoundingBox { get; set; }

            public Node()
            {
                Children = new List<Node>();
                BoundingBox = Rectangle.Empty;
            }

            public Node(T obj)
            {
                Object = obj;
                Children = new List<Node>();
                BoundingBox = obj.BoundingBox;
            }

            public Node(Rectangle boundingBox)
            {
                Children = new List<Node>();

                BoundingBox = boundingBox;
            }
        }

        private Node root;

        private int maxChildren;
        private double spreadFactor;

        public RTree(int maxChildren = 5, double spreadFactor = 0.6)
        {
            this.maxChildren = maxChildren;
            this.spreadFactor = spreadFactor;

            root = new Node(new Rectangle(double.NegativeInfinity,  // MinX
                                          double.PositiveInfinity,  // MaxX
                                          double.NegativeInfinity,  // MinY
                                          double.PositiveInfinity)  // MaxY
            );
        }

        public void Insert(T obj)
        {
            InsertNode(root, new Node(obj));
        }

        void InsertNode(Node node, Node newNode)
        {
            Rectangle union = Rectangle.Union(node.BoundingBox, newNode.BoundingBox);
            double area = union.Area;

            IEnumerable<Node> childrenByArea = node.Children.OrderBy(child => Rectangle.Union(child.BoundingBox, newNode.BoundingBox).Area);

            Node? bestChild = childrenByArea.FirstOrDefault();

            if (bestChild is not null)
            {
                Rectangle childUnion = Rectangle.Union(bestChild.BoundingBox, newNode.BoundingBox);

                double newNodeArea = newNode.BoundingBox.Area;
                double childArea = bestChild.BoundingBox.Area;
                double unionArea = childUnion.Area;

                if (unionArea < spreadFactor * area)
                {
                    InsertNode(bestChild, newNode);

                    return;
                }
            }

            if (node.Object is not null)
            {
                Node newChild = new Node(node.Object);
                node.Object = default(T);
                node.Children.Add(newChild);
            }

            node.Children.Add(newNode);
            node.BoundingBox = union;

            if (node.Children.Count > maxChildren)
            {
                ReorderChildren(node);
            }
        }

        public IEnumerable<T> Query(Rectangle queryBox)
        {
            List<T> result = new List<T>();

            QueryNode(root, queryBox, ref result);

            return result;
        }

        void QueryNode(Node node, Rectangle queryBox, ref List<T> result)
        {
            if (!node.BoundingBox.Overlaps(queryBox)) return;

            if (node.Object is not null)
            {
                result.Add(node.Object);
            }

            foreach (Node child in node.Children)
            {
                QueryNode(child, queryBox, ref result);
            }
        }

        void ReorderChildren(Node node)
        {
            Node leftNode = new Node();
            Node rightNode = new Node();

            Queue<Node> queue = new Queue<Node>(Randomizer.Shuffle(node.Children));

            while (queue.Count > 0)
            {
                Node child = queue.Dequeue();

                Rectangle leftUnion = Rectangle.Union(leftNode.BoundingBox, child.BoundingBox);
                Rectangle rightUnion = Rectangle.Union(rightNode.BoundingBox, child.BoundingBox);

                if (leftUnion.Area < rightUnion.Area)
                {
                    leftNode.Children.Add(child);
                    leftNode.BoundingBox = leftUnion;
                }
                else
                {
                    rightNode.Children.Add(child);
                    rightNode.BoundingBox = rightUnion;
                }
            }

            node.Children.RemoveAll(_ => true);
            node.Children.Add(leftNode);
            node.Children.Add(rightNode);
        }
    }
}