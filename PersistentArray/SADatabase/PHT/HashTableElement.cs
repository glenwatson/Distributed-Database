using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByteHelper;
using Interfaces;
using PA;

namespace PHT
{
    public class HashTableElement : Element, ISerializeable, IComparable<HashTableElement>
    {
        public byte[] Key { get; set; }
        public int Index { get; set; }

        public byte[] Value
        {
            get { return Data; }
            set { Data = value; }
        }

        public HashTableElement(byte[] key, byte[] value, int index) : base(value)
        {
            Index = index;
            Key = key;
        }

        public new byte[] Serialize()
        {
            byte[] serialized = Key.Append(Data);
            return serialized;
        }

        public static HashTableElement Deserialize(byte[] data, int index, int keySize, int valueSize)
        {
            if(data.Length < keySize+valueSize)
                throw new InsufficientDataException("Not enough data to deserialize");

            byte[] key = data.SubArray(0, keySize);
            byte[] value = data.SubArray(keySize, keySize+valueSize);
            return new HashTableElement(key, value, index);
        }

        public int CompareTo(HashTableElement other)
        {
            return this.Key.GetHash() - other.Key.GetHash();
        }

        public override bool Equals(object obj)
        {
            if (obj is HashTableElement)
                return CompareTo((HashTableElement) obj) == 0;
            return false;
        }
    }
}
