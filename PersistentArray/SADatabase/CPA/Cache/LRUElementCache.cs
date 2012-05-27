using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CPA.Cache.LinkedList;
using PA;

namespace CPA.Cache
{
    public class LRUElementCache
    {
        private readonly LRUDoublyLinkedList _cacheList;
        private readonly Dictionary<int, Node> _index;
        private readonly int _cacheSize;
        public int Size { get { return _cacheSize; } }

        public LRUElementCache(int size)
        {
            _cacheSize = size;
            _cacheList = new LRUDoublyLinkedList();
            _index = new Dictionary<int, Node>();
        }

        public Element Get(int elementIdx)
        {
            Node cachedNode;
            _index.TryGetValue(elementIdx, out cachedNode);
            Element result = null;
            if(cachedNode != null)
                result = cachedNode.Element;
            return result;
        }

        public void AddToCache(Element element, int elementIdx)
        {
            Node cachedNode;
            if (_index.TryGetValue(elementIdx, out cachedNode))
            {
                cachedNode.SetElement(element);
                _cacheList.MoveToHead(cachedNode);
            }
            else
            {
                Node node = new Node(null, null, element, elementIdx);
                _cacheList.Insert(node);
                _index.Add(elementIdx, node);
            }
            if (_cacheList.Size > _cacheSize)
                RemoveLast();
        }

        private Element RemoveLast()
        {
            Node removedNode = _cacheList.RemoveEnd();
            _index.Remove(removedNode.ElementIndex);
            return removedNode.Element;
        }
    }
}
