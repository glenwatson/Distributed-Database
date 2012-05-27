using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByteHelper;
using Interfaces;
using PA;
using PA.Exceptions;
using Persistence;

namespace PAFS
{
    public class PAFSHeader : ISerializeable
    {
        public int NextFreeSpaceIndex { get; set; }

        public PAFSHeader(int fsIndex)
        {
            NextFreeSpaceIndex = fsIndex;
        }

        public byte[] Serialize()
        {
            return NextFreeSpaceIndex.ToBytes();
        }

        public static PAFSHeader Deserialize(byte[] data)
        {
            if (data.Length < GetHeaderSize())
                throw new InsufficientDataException("The data supplied was not enough to deserialize");
            return new PAFSHeader(data.ToInt());
        }

        public static int GetHeaderSize()
        {
            return PersistenceConstants.IntSize;
        }
    }
}
