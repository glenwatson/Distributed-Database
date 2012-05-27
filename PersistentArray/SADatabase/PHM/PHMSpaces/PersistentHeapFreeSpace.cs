using ByteHelper;
using PHM.Exceptions;

namespace PHM.PHMSpaces
{
    public class PersistentHeapFreeSpace : PersistentHeapSpace
    {
        public int NextFreeSpace { get; set; }

        public PersistentHeapFreeSpace(int sizeIndex, int endIndex, int nextFreeSpace) : base(sizeIndex, endIndex)
        {
            NextFreeSpace = nextFreeSpace;
        }

        public PersistentHeapUsedSpace Split(int newUserSize)
        {
            int newProposedBoundery = StartIndex + newUserSize-1;
            if(newProposedBoundery <= StartIndex)
                throw new InvalidIndexException("The new size is too small for a space to be created before it");
            if(newProposedBoundery >= EndIndex - GetUserSizeSize())
                throw new InvalidIndexException("The new size is too large for a space to be created after it");

            int usedSpaceStartIndex = newProposedBoundery + 1;
            int usedSpaceEndIndex = EndIndex;
            PersistentHeapUsedSpace usedSpace = new PersistentHeapUsedSpace(usedSpaceStartIndex, usedSpaceEndIndex, new byte[0]);

            EndIndex = newProposedBoundery;

            return usedSpace;
        }

        public void MergeBefore(PersistentHeapFreeSpace before)
        {
            Merge(before, this);
            SizeIndex = before.SizeIndex;
            EndIndex = before.EndIndex;
            NextFreeSpace = before.NextFreeSpace;
            NullOut(before);

        }

        public void MergeAfter(PersistentHeapFreeSpace after)
        {
            Merge(this, after);
            NullOut(after);
        }

        private static void Merge(PersistentHeapFreeSpace before, PersistentHeapFreeSpace after)
        {
            AssertHeapSpaceComesRightBefore(before, after);
            before.EndIndex = after.EndIndex;
        }

        private static void NullOut(PersistentHeapFreeSpace space)
        {
            space.SizeIndex = -1;
            space.EndIndex = -1;
            space.NextFreeSpace = -2;
        }

        public override byte[] Serialize()
        {
            byte[] bytes = UserSize.ToBytes().Append(NextFreeSpace.ToBytes().ExtendTo(UserSize));
            return bytes;
        }
    }
}
