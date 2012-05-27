using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByteHelper;
using Interfaces;
using PA;
using PA.Exceptions;
using Persistence;

namespace PLL
{
    public class PLLHeader : ISerializeable
    {
        public int Size { get; set; }

        public PLLHeader(int size)
        {
            Size = size;
        }

        public byte[] Serialize()
        {
            return Size.ToBytes();
        }

        public static PLLHeader Deserialize(byte[] data)
        {
            if(data.Length < GetDataSize())
                throw new InsufficientDataException("The data passed is not large enough to deserialize from");
            return new PLLHeader(data.ToInt());
        }

        public static int GetDataSize()
        {
            return PersistenceConstants.IntSize;
        }
    }
}
