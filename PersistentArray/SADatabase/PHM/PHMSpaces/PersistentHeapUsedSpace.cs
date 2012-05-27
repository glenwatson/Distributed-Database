using System;
using System.IO;
using ByteHelper;

namespace PHM.PHMSpaces
{
    public class PersistentHeapUsedSpace : PersistentHeapSpace
    {
        public byte[] Data { get; set; }

        public PersistentHeapUsedSpace(int sizeIndex, int endIndex, byte[] data)
            : base(sizeIndex, endIndex)
        {
            if (data.Length > UserSize)
                throw new IndexOutOfRangeException("The data given is too large for the space");
            Data = data;
        }

        public override byte[] Serialize()
        {
            byte[] bytes = UserSize.ToBytes().Append(Data).ExtendTo(SizeInBytes);
            return bytes;
        }
    }
}
