using PA;

namespace CPA.Cache.LinkedList
{
    public class Node
    {
        public Element Element { get; private set; }
        public int ElementIndex { get; private set; }

        public Node Child { get; private set; }
        public Node Parent { get; private set; }

        public Node(Node child, Node parent, Element element, int eleIdx)
        {
            Child = child;
            Parent = parent;

            Element = element;
            ElementIndex = eleIdx;
        }

        public void SetChild(Node child)
        {
            Child = child;
        }

        public void SetParent(Node parent)
        {
            Parent = parent;
        }

        public void SetElement(Element element)
        {
            Element = element;
        }
    }
}
