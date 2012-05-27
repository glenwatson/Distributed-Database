using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByteHelper;
using Interfaces;
using Persistence;

namespace PHM
{
    public class PersistentHeapHeader : ISerializeable
    {
        public int FreeSpaceHead { get; set; }
        public static int NullPointer { get { return -1; } }

        public PersistentHeapHeader(int freeSpaceHead)
        {
            FreeSpaceHead = freeSpaceHead;
        }

        public byte[] Serialize()
        {
            return FreeSpaceHead.ToBytes();
        }

        public static PersistentHeapHeader Deserialize(byte[] data)
        {
            if (data.Length < GetDataSize())
                throw new InsufficientDataException("The data passed is not large enough to deserialize from");
            return new PersistentHeapHeader(data.ToInt());
        }

        public static int GetDataSize()
        {
            return PersistenceConstants.IntSize;
        }
    }
}
