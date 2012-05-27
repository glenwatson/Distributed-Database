using System.Diagnostics;
using PA.Exceptions;

namespace CPA.Cache.LinkedList
{
    public class LRUDoublyLinkedList
    {
        private Node _head;
        private Node _tail;
        public int Size { get; private set; }

        public Node Get(int index)
        {
            if(index < 0)
                throw new InvalidElementIndexException("The index must be positive");
            Node node = _head;
            int count = 0;
            while (node != null && count < index)
            {
                node = node.Child;
                count++;
            }

            if(node != null && node != _head)
                MoveToHead(node);
            return node;
        }

        public void Insert(Node n)
        {
            InsertAtHead(n);
            Size++;
        }

        private void InsertAtHead(Node n)
        {
            n.SetParent(null);
            n.SetChild(_head);

            if (_head == null) // no nodes yet
            {
                Debug.Assert(_tail == null);
                _tail = n;
            }
            if (_head != null) // at least one node in the list
            {
                _head.SetParent(n);
            }
            _head = n;

        }

        public void MoveToHead(Node n)
        {
            if (n == _head)
            {
                //already the _head
            }
            else if (n == _tail) // the node to move is the tail
            {
                Debug.Assert(_tail.Parent != null);
                _tail.Parent.SetChild(null);
                _tail = _tail.Parent;
                InsertAtHead(n);
            }
            else // the node is in the middle
            {
                n.Parent.SetChild(n.Child);
                n.Child.SetParent(n.Parent);
                InsertAtHead(n);
            }
            
        }

        public Node RemoveEnd()
        {
            Node oldTail = _tail;
            if (_tail == null) // no nodes
            {
                return null;
            }
            else if (_head == _tail) //one node
            {
                _head = null;
                _tail = null;
            }
            else //two or more nodes
            {
                Node secondLast = _tail.Parent;
                _tail = secondLast;
                _tail.SetChild(null);
            }

            Size--;

            oldTail.SetChild(null);
            oldTail.SetParent(null);
            return oldTail;
        }
    }
}
