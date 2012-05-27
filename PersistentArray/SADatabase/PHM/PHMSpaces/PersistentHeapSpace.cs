using ByteHelper;
using Interfaces;
using PHM.Exceptions;
using Persistence;

namespace PHM.PHMSpaces
{
    public abstract class PersistentHeapSpace : ISerializeable
    {
        public int SizeIndex { get; set; }
        public int EndIndex { get; set; }
        public int StartIndex { get { return SizeIndex + GetUserSizeSize(); } }
        public int UserSize { get { return EndIndex - StartIndex + 1; } }
        public int SizeInBytes { get { return EndIndex - SizeIndex + 1; } }

        protected PersistentHeapSpace(int sizeIndex, int endIndex)
        {
            if (sizeIndex >= endIndex)
                throw new InvalidIndexException("endIndex must come after sizeIndex");
            SizeIndex = sizeIndex;
            EndIndex = endIndex;
        }

        protected static void AssertHeapSpaceComesRightBefore(PersistentHeapSpace before, PersistentHeapSpace after)
        {
            if (before.EndIndex + 1 != after.SizeIndex)
                throw new MemoryNotContinuousException("The memory spaces provided are not continuous");
        }

        public static int GetUserSizeSize()
        {
            return PersistenceConstants.IntSize;
        }

        public static int GetUserSize(byte[] userSizeBytes)
        {
            return userSizeBytes.ToInt();
        }

        public bool IsDirectlyBefore(PersistentHeapSpace space)
        {
            return EndIndex + 1 == space.SizeIndex;
        }

        public bool IsDirectlyAfter(PersistentHeapSpace space)
        {
            return space.EndIndex + 1 == SizeIndex;
        }

        public abstract byte[] Serialize();
    }
}
