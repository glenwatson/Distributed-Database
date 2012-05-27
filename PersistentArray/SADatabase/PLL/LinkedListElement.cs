using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByteHelper;
using Interfaces;
using PA;
using Persistence;

namespace PLL
{
    public class LinkedListElement : Element, ISerializeable
    {
        public int Next { get; set; }
        public int Previous { get; set; }
        public int Index { get; private set; }

        public LinkedListElement(byte[] data, int next, int previous, int index) : base(data)
        {
            Next = next;
            Previous = previous;
            Index = index;
        }

        public new byte[] Serialize()
        {
            byte[] elementBytes = Next.ToBytes().Append(Previous.ToBytes(), Data);
            return elementBytes;
        }

        public static LinkedListElement Deserialize(byte[] get, int index)
        {
            int next = get.SubArray(0, PersistenceConstants.IntSize).ToInt();
            int prev = get.SubArray(PersistenceConstants.IntSize, GetMinimumElementSize()).ToInt();
            byte[] data = get.SubArray(GetMinimumElementSize(), get.Length);
            return new LinkedListElement(data, next, prev, index);
        }

        public static int GetMinimumElementSize()
        {
            return 2 * PersistenceConstants.IntSize;
        }
    }
}
