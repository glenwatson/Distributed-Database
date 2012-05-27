using ByteHelper;
using Interfaces;
using PA.Exceptions;
using PA;
using Persistence;

namespace PAFS
{
    public class PersistentCollectionSpaceManager : IPersistentCollectionSpaceManager
    {
        private readonly IPersistentArrayNextSpace _simpleCollectionNextSpace;
        private const int FreeSpaceTailIndicator = -1;

        #region Proxy

        public PersistentCollectionSpaceManager(string arrayName, int elementSize, int userHeaderSize)
        {
            AssertElementSize(elementSize);
            _simpleCollectionNextSpace = new PersistentNextSpaceArray(arrayName, elementSize, userHeaderSize + GetFreeSpaceSize());
            PutNextFreeSpaceIndex(FreeSpaceTailIndicator);
            PutUserHeader(new byte[0]);
        }

        public PersistentCollectionSpaceManager(string simpleCollection)
        {
            _simpleCollectionNextSpace = new PersistentNextSpaceArray(simpleCollection);
            AssertElementSize(_simpleCollectionNextSpace.GetElementSize());
        }

        private static void AssertElementSize(int elementSize)
        {
            if (elementSize < PersistenceConstants.IntSize)
                throw new InvalidElementSizeException("Can't have an element size smaller than " + PersistenceConstants.IntSize);
        }

        private void PutNextFreeSpaceIndex(int nextFreeSpaceIndex)
        {
            _simpleCollectionNextSpace.PutUserHeader(nextFreeSpaceIndex.ToBytes().Append(GetUserHeader()));
        }

        private int GetNextFreeSpaceIndex()
        {
            byte[] fullUserHeader = _simpleCollectionNextSpace.GetUserHeader();
            byte[] nextFreeSpaceIndexBytes = fullUserHeader.SubArray(0, GetNextFreeSpaceIndexsize());
            return nextFreeSpaceIndexBytes.ToInt();
        }

        private int GetNextFreeSpaceIndexsize()
        {
            return PersistenceConstants.IntSize;
        }

        public void WipeElement(int index)
        {
            _simpleCollectionNextSpace.Put(index, new byte[GetElementSize()]);
        }

        public int GetElementSize()
        {
            return _simpleCollectionNextSpace.GetElementSize();
        }

        public byte[] Get(int index)
        {
            return _simpleCollectionNextSpace.Get(index);
        }

        public void Put(int index, byte[] buffer)
        {
            _simpleCollectionNextSpace.Put(index, buffer);
        }

        public void Delete()
        {
            _simpleCollectionNextSpace.Delete();
        }

        public void Close()
        {
            _simpleCollectionNextSpace.Close();
        }

        #endregion

        public int AllocateBlock()
        {
            int indexToAllocate;
            if (GetNextFreeSpaceIndex() == FreeSpaceTailIndicator)
            {
                indexToAllocate = _simpleCollectionNextSpace.GetNextIndex();
            }
            else
            {
                indexToAllocate = PopFreeSpaceOffStack();
            }
            return indexToAllocate;
        }

        private int PopFreeSpaceOffStack()
        {
            int nextAvailableBlock = GetNextFreeSpaceIndex();
            //pop arrayIndex off stack
            int newNextFreeSpace = _simpleCollectionNextSpace.Get(nextAvailableBlock).ToInt();
            PutNextFreeSpaceIndex(newNextFreeSpace);
            _simpleCollectionNextSpace.WipeElement(nextAvailableBlock);
            WriteOutHeader();
            return nextAvailableBlock;
        }

        public void FreeBlock(int blockIndex)
        {
            int indexToWriteToBlockIndex; //what to write to the new tail
            if (GetNextFreeSpaceIndex() == FreeSpaceTailIndicator) // initalize the stack
            {
                //set tail to null
                indexToWriteToBlockIndex = FreeSpaceTailIndicator;
            }
            else //push to stack
            {
                indexToWriteToBlockIndex = blockIndex;
            }
            byte[] blockIndexBytes = indexToWriteToBlockIndex.ToBytes();

            byte[] newElement = new byte[_simpleCollectionNextSpace.GetElementSize()];
            blockIndexBytes.CopyInto(newElement);
            _simpleCollectionNextSpace.Put(blockIndex, newElement);

            //update memory
            PutNextFreeSpaceIndex(blockIndex);
        }

        private void WriteOutHeader()
        {
            PutUserHeader(GetUserHeader());
        }

        #region Modified Proxy

        public byte[] GetUserHeader()
        {
            byte[] fullUHeader = _simpleCollectionNextSpace.GetUserHeader();
            return fullUHeader.SubArray(GetFreeSpaceSize(), fullUHeader.Length);
        }

        //private byte[] GetHeader()
        //{
        //    byte[] fullUHeader = _simpleCollectionNextSpace.GetUserHeader();
        //    return fullUHeader.SubArray(0, GetFreeSpaceSize());
        //}

        public int GetUserHeaderSize()
        {
            return _simpleCollectionNextSpace.GetUserHeaderSize() - GetFreeSpaceSize();
        }

        private static int GetFreeSpaceSize()
        {
            return PersistenceConstants.IntSize;
        }

        public void PutUserHeader(byte[] userHeader)
        {
            //combine headers
            byte[] newUserHeader = GetNextFreeSpaceIndex().ToBytes().Append(userHeader);
            //forward it on
            _simpleCollectionNextSpace.PutUserHeader(newUserHeader);
        }

        #endregion

        public int GetNextIndex()
        {
            return AllocateBlock();
        }
    }
}
