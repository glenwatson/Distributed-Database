using System;
using System.Diagnostics;
using System.IO;
using ByteHelper;
using FilePersistence;
using FilePersistence.Exceptions;
using Interfaces;
using PA.Exceptions;
using PA;
using Persistence;

namespace PA
{
    public class PersistentNextSpaceArray : IPersistentArrayNextSpace
    {
        private FileWithHeader _fileWithHeader;

        public PersistentNextSpaceArray(String arrayName, int elementSize, int userHeaderSize)
        {
            if (elementSize <= 0)
                throw new InvalidElementSizeException("The element size must be greater than zero");
            if(userHeaderSize < 0)
                throw new InvalidUserHeaderException("User Header size must be positive");
            _fileWithHeader = new FileWithHeader(arrayName, userHeaderSize + GetElementSizeSize());
            PutElementSize(elementSize);
            PutUserHeader(new byte[0]);
        }

        public PersistentNextSpaceArray(String arrayName)
        {
            _fileWithHeader = new FileWithHeader(arrayName);
            byte[] userHeaderBytes = _fileWithHeader.GetUserHeader();
        }

        public void Delete()
        {
            _fileWithHeader.Delete();
        }

        public int GetElementSize()
        {
            byte[] fullUserHeader = _fileWithHeader.GetUserHeader();
            byte[] elementSizeBytes = fullUserHeader.SubArray(0, GetElementSizeSize());
            return elementSizeBytes.ToInt();
        }

        private void PutElementSize(int elementSize)
        {
            _fileWithHeader.PutUserHeader(elementSize.ToBytes().Append(GetUserHeader()));
        }

        public void WipeElement(int index)
        {
            AssertElementIndexIsValid(index);
            byte[] zeros = new byte[GetElementSize()];
            Put(index, zeros);
        }

        public byte[] Get(int index)
        {
            AssertElementIndexIsValid(index);
            if(index >= GetNextIndex())
                throw new InvalidElementIndexException("index is outside the file");

            int byteIndex = GetByteIndex(index);
            return _fileWithHeader.GetAmount(byteIndex, GetElementSize());
        }

        public void Put(int index, byte[] buffer)
        {
            AssertElementIndexIsValid(index);
            int elementSize = GetElementSize();
            if (buffer.Length > elementSize)
            {
                throw new InternalBufferOverflowException("Buffer is too large to put");
            }

            byte[] elementBytes = buffer.ExtendTo(elementSize);
            Debug.Assert(elementBytes.Length == elementSize);
            int byteIndex = GetByteIndex(index);
            _fileWithHeader.Put(byteIndex,elementBytes);
        }


        private int GetByteIndex(int index)
        {
            //the fileWithHeader takes care of the offset from the userHeader
            //and his user header contains your user header data too, so you 
            //don't need to include yours in the calculation. Just take care 
            //of the element
            return index*GetElementSize();
        }

        private void AssertElementIndexIsValid(int elementIndex)
        {
            if(elementIndex < 0)
                throw new InvalidElementIndexException("The index must be greater than than zero");
        }

        public byte[] GetUserHeader()
        {
            byte[] fullUserHeader = _fileWithHeader.GetUserHeader();
            byte[] publicUserHeader = fullUserHeader.SubArray(GetElementSizeSize(), fullUserHeader.Length);
            return publicUserHeader;
        }

        private static int GetElementSizeSize()
        {
            return PersistenceConstants.IntSize;
        }

        public int GetUserHeaderSize()
        {
            return _fileWithHeader.GetUserHeaderSize() - GetElementSizeSize();
        }

        public void PutUserHeader(byte[] userHeader)
        {
            _fileWithHeader.PutUserHeader(GetElementSize().ToBytes().Append(userHeader));
        }

        public void Close()
        {
            _fileWithHeader.Close();
        }

        public int GetNextIndex()
        {
            double actualNextIndex = _fileWithHeader.GetNextIndex();
            double nextElement = actualNextIndex / GetElementSize();
            double roundedUp = Math.Ceiling(nextElement);
            return (int) roundedUp;
        }
    }
}
