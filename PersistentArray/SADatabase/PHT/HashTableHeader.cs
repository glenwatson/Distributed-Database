using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByteHelper;
using Interfaces;
using PA;
using PA.Exceptions;
using Persistence;

namespace PHT
{
    public class HashTableHeader : ISerializeable
    {
        public int TableSize { get; set; }
        public int KeySize { get; set; }
        public int ValueSize { get; set; }

        public HashTableHeader(int tableSize, int keySize, int valueSize)
        {
            TableSize = tableSize;
            KeySize = keySize;
            ValueSize = valueSize;
        }

        public byte[] Serialize()
        {
            byte[] serialized = TableSize.ToBytes().Append(KeySize.ToBytes(), ValueSize.ToBytes());
            return serialized;
        }

        public static HashTableHeader Deserialize(byte[] headerData)
        {
            if (headerData.Length < GetHeaderSize())
                throw new InsufficientDataException("Not enough data in headerData to deserialize");
            int tableSize = headerData.SubArray(0, PersistenceConstants.IntSize).ToInt();
            int keySize = headerData.SubArray(PersistenceConstants.IntSize, 2 * PersistenceConstants.IntSize).ToInt();
            int valueSize = headerData.SubArray(2 * PersistenceConstants.IntSize, GetHeaderSize()).ToInt();
            return new HashTableHeader(tableSize, keySize, valueSize);
        }

        public static int GetHeaderSize()
        {
            return 3 * PersistenceConstants.IntSize;
        }
    }
}
