using System;
using ByteHelper;
using Interfaces;
using PAFS;
using PLL.Exceptions;
using Persistence;


namespace PLL
{
    public class PersistentLinkedList : ILinkedList
    {
        private readonly IPersistentCollectionSpaceManager _persistentSimpleCollection;

        public PersistentLinkedList(String listName, int elementSize, int userHeaderSize)
        {
            _persistentSimpleCollection = new PersistentCollectionSpaceManager(listName, elementSize + 2 * PersistenceConstants.IntSize, userHeaderSize+GetSizeSize());
            _persistentSimpleCollection.Put(0, new byte[0]); // I don't want the first element to be returned for GetNextIndex()
        }

        public PersistentLinkedList(string llName)
        {
            _persistentSimpleCollection = new PersistentCollectionSpaceManager(llName);
        }

        public void Delete()
        {
            _persistentSimpleCollection.Delete();
        }

        public void Close()
        {
            _persistentSimpleCollection.Close();
        }

        private LinkedListElement GetNullElement()
        {
            return GetElementAt(0);
        }

        public int AddToStart(byte[] buffer)
        {
            LinkedListElement insertingNode = GetNewElement(buffer);
            Link(GetNullElement(), insertingNode, GetHead());
            IncreaseSize();
            return insertingNode.Index;
        }

        public int AddToEnd(byte[] buffer)
        {
            LinkedListElement insertingNode = GetNewElement(buffer);
            Link(GetTail(), insertingNode, GetNullElement());
            IncreaseSize();
            return insertingNode.Index;
        }

        public int AddBefore(int nodeReference, byte[] buffer)
        {
            AssertNotBadNodeReference(nodeReference);
            LinkedListElement nodeReferenceElement = GetElementAt(nodeReference);
            return AddAfter(GetElementAt(nodeReferenceElement.Previous), buffer);
        }

        public int AddAfter(int nodeReference, byte[] buffer)
        {
            AssertNotBadNodeReference(nodeReference);
            LinkedListElement nodeReferenceElement = GetElementAt(nodeReference);
            return AddAfter(nodeReferenceElement, buffer);
        }

        private int AddAfter(LinkedListElement previous, byte[] buffer)
        {
            LinkedListElement insertingNode = GetNewElement(buffer);
            if (previous.Index == insertingNode.Index)
                throw new InvalidNodeReference("The node reference passed is not valid");
            LinkedListElement next = GetElementAt(previous.Next);
            Link(previous, insertingNode, next);
            IncreaseSize();
            return insertingNode.Index;
        }

        private void Link(LinkedListElement previous, LinkedListElement middle, LinkedListElement next)
        {
            SetPreviousNext(previous, middle);

            if (previous.Index == next.Index)
            {
                SetPreviousNext(middle, previous);
            }
            else
            {
                SetPreviousNext(middle, next);
                PutElement(next);
            }

            PutElement(previous);
            PutElement(middle);
        }

        private void SetPreviousNext(LinkedListElement previous, LinkedListElement next)
        {
            previous.Next = next.Index;
            next.Previous = previous.Index;
        }

        private LinkedListElement GetNewElement(byte[] buffer)
        {
            return new LinkedListElement(buffer, -1, -1, _persistentSimpleCollection.AllocateBlock());
        }

        private LinkedListElement GetHead()
        {
            LinkedListElement nullElement = GetNullElement();
            if (nullElement.Next == nullElement.Index)
                return nullElement;
            return GetElementAt(nullElement.Next);
        }

        private LinkedListElement GetTail()
        {
            LinkedListElement nullElement = GetNullElement();
            if (nullElement.Previous == nullElement.Index)
                return nullElement;
            return GetElementAt(nullElement.Previous);
        }

        private void PutElement(LinkedListElement element)
        {
            byte[] elementBytes = element.Serialize();
            _persistentSimpleCollection.Put(element.Index, elementBytes);
        }

        public void Update(int nodeReference, byte[] buffer)
        {
            AssertNotBadNodeReference(nodeReference);
            LinkedListElement updatingElement = GetElementAt(nodeReference);
            updatingElement.Data = buffer;
            PutElement(updatingElement);
        }

        #region Gets

        public int GetFirst()
        {
            return GetElementAt(0).Next;
        }

        public int GetLast()
        {
            return GetElementAt(0).Previous;
        }

        public int GetNext(int nodeReference)
        {
            AssertNotBadNodeReference(nodeReference);
            return GetElementAt(nodeReference).Next;
        }

        public int GetPrevious(int nodeReference)
        {
            AssertNotBadNodeReference(nodeReference);
            return GetElementAt(nodeReference).Previous;
        }

        public byte[] GetData(int nodeReference)
        {
            AssertNotBadNodeReference(nodeReference);
            return GetElementAt(nodeReference).Data;
        }

        private LinkedListElement GetElementAt(int arrayIndex)
        {
            LinkedListElement lle = LinkedListElement.Deserialize(_persistentSimpleCollection.Get(arrayIndex), arrayIndex);
            return lle;
        }

        #endregion

        public byte[] Remove(int nodeReference)
        {
            if (GetSize() == 0) //um, there's nothing in the list?
            {
                throw new InvalidOperationException("No elements in list");
            }
            AssertNotBadNodeReference(nodeReference);
            LinkedListElement removingElement = GetElementAt(nodeReference);
            LinkedListElement previous = GetElementAt(removingElement.Previous);
            LinkedListElement next = GetElementAt(removingElement.Next);
            SetPreviousNext(previous, next);
            PutElement(previous);
            PutElement(next);
            
            DecreaseSize();

            _persistentSimpleCollection.FreeBlock(removingElement.Index);
            return removingElement.Data;
        }

        private void AssertNotBadNodeReference(int nodeReference)
        {
            if (nodeReference < 0)
            {
                throw new InvalidNodeReference("Invalid node reference: "+nodeReference);
            }
        }

        public int Length()
        {
            return GetSize();
        }

        public int GetElementSize()
        {
            return _persistentSimpleCollection.GetElementSize() - GetLinkListPointersSize();
        }

        private static int GetLinkListPointersSize()
        {
            return 2 * PersistenceConstants.IntSize;
        }
        
        #region UserHeader
        public int GetUserHeaderSize()
        {
            return _persistentSimpleCollection.GetUserHeaderSize() - GetSizeSize();
        }

        public void PutUserHeader(byte[] userHeader)
        {
            _persistentSimpleCollection.PutUserHeader(GetSize().ToBytes().Append(userHeader));
        }

        public byte[] GetUserHeader()
        {
            byte[] fullUserHeader = _persistentSimpleCollection.GetUserHeader();
            byte[] userHeaderBytes = fullUserHeader.SubArray(GetSizeSize(), fullUserHeader.Length);
            return userHeaderBytes;
        }

        public int GetSize()
        {
            byte[] fullUserHeader = _persistentSimpleCollection.GetUserHeader();
            byte[] sizeBytes = fullUserHeader.SubArray(0, GetSizeSize());
            return sizeBytes.ToInt();
        }

        private int GetSizeSize()
        {
            return PersistenceConstants.IntSize;
        }

        private void IncreaseSize()
        {
            int size = GetSize();
            size++;
            _persistentSimpleCollection.PutUserHeader(size.ToBytes().Append(GetUserHeader()));
        }

        private void DecreaseSize()
        {
            int size = GetSize();
            size--;
            _persistentSimpleCollection.PutUserHeader(size.ToBytes().Append(GetUserHeader()));
        }
        #endregion
    }
}
