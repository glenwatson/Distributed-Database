using System;
using System.Collections.Generic;
using System.Diagnostics;
using ByteHelper;
using FilePersistence;
using PA.Exceptions;
using PHM.Exceptions;
using PHM.PHMSpaces;
using Persistence;

namespace PHM
{
    public class PersistentHeap : IHeap
    {
        private readonly FileWithHeader _file;

        private static int FreeLinkedListNullPointer
        {
            get { return -1; }
        }

        #region Init
        public PersistentHeap(String heapName, int userHeaderSize)
        {
            _file = new FileWithHeader(heapName, userHeaderSize + GetFreeSpaceHeadSize());
            PutFreeSpaceHead(FreeLinkedListNullPointer);
        }

        public PersistentHeap(String heapName)
        {
            _file = new FileWithHeader(heapName);
        }

        public void Delete()
        {
            _file.Delete();
        }
        #endregion

        #region Allocate
        public int Allocate(int allocationSize)
        {
            AssertUserSize(allocationSize);
            PersistentHeapFreeSpace freeSpot = GetFreeSpaceThatIsAtLeast(allocationSize);
            PersistentHeapUsedSpace usedSpot;
            if (freeSpot.UserSize > allocationSize)
            {
                usedSpot = freeSpot.Split(allocationSize);
                PutFullSpace(freeSpot);
            }
            else //the freeSpot is the perfect size
            {
                Debug.Assert(freeSpot.UserSize == allocationSize);
                usedSpot = ToUsedSpace(freeSpot);
            }
            PutFullSpace(usedSpot);
            Debug.Assert(usedSpot.UserSize == allocationSize);
            return usedSpot.StartIndex;
        }

        private void AssertUserSize(int allocationSize)
        {
            if (allocationSize < PersistenceConstants.IntSize)
                throw new InvalidElementSizeException("The heap space size is too small");
        }

        private PersistentHeapUsedSpace ToUsedSpace(PersistentHeapFreeSpace freeSpace)
        {
            // remove free space from linked list of free spaces 
            PersistentHeapFreeSpace before = GetFreeSpaceBefore(freeSpace.StartIndex);


            if (before == null) //there isn't free space before freeSpace
            {
                Debug.Assert(GetFreeSpaceHead() == freeSpace.StartIndex); //the first free space better be the free space passed in
                PutFreeSpaceHead(freeSpace.NextFreeSpace);
            }
            else //there is a free space before freeSpace
            {
                Debug.Assert(before.NextFreeSpace == freeSpace.StartIndex);                
                before.NextFreeSpace = freeSpace.NextFreeSpace;
                PutFullSpace(before);
            }

            int usedSpaceDataSize = freeSpace.EndIndex - freeSpace.StartIndex + 1;
            PersistentHeapUsedSpace usedSpace = new PersistentHeapUsedSpace(freeSpace.SizeIndex, freeSpace.EndIndex, new byte[usedSpaceDataSize]);
            PutFullSpace(usedSpace);
            return usedSpace;
        }

        private PersistentHeapFreeSpace GetFreeSpaceBefore(int nextIndex)
        {
            //if (nextIndex == GetFreeLinkedListNullPointer()) // nextIndex is at the end of the list
            //{
            //    nextIndex = int.MaxValue;
            //}
            foreach (PersistentHeapFreeSpace persistentHeapFreeSpace in GetFreeSpaces())
            {
                if (persistentHeapFreeSpace.NextFreeSpace >= nextIndex)
                    return persistentHeapFreeSpace;
            }

            return null;
        }

        private PersistentHeapFreeSpace GetFreeSpaceThatIsAtLeast(int userSize)
        {
            PersistentHeapFreeSpace lastFreeSpace = null;
            //try to find a space in the free spaces list
            foreach (PersistentHeapFreeSpace persistentHeapFreeSpace in GetFreeSpaces())
            {
                if (persistentHeapFreeSpace.UserSize >= userSize)
                    return persistentHeapFreeSpace;
                lastFreeSpace = persistentHeapFreeSpace;
            }


            //couldn't freespace available that is big enough, going to make our own

            int openIndex = _file.GetNextIndex();
            int fullSize = userSize + PersistentHeapSpace.GetUserSizeSize();
            PersistentHeapFreeSpace freeSpaceOnTheEnd = new PersistentHeapFreeSpace(openIndex, openIndex + fullSize-1, FreeLinkedListNullPointer);

            //update the free linked list
            if (lastFreeSpace == null) //there are no nodes in the free space linked list
            {
                Debug.Assert(GetFreeSpaceHead() == FreeLinkedListNullPointer);
                PutFreeSpaceHead(freeSpaceOnTheEnd.StartIndex);
            }
            else //there are nodes in the free space linked list
            {
                Debug.Assert(lastFreeSpace != null);
                lastFreeSpace.NextFreeSpace = freeSpaceOnTheEnd.StartIndex;
                PutFullSpace(lastFreeSpace);
            }

            PutFullSpace(freeSpaceOnTheEnd);
            return freeSpaceOnTheEnd;
        }
        #endregion

        #region Free
        public void Free(int token)
        {
            //free the space
            PersistentHeapUsedSpace usedSpace = GetUsedSpaceAt(token);
            usedSpace.Data = new byte[usedSpace.UserSize];

            PersistentHeapFreeSpace freedSpace = new PersistentHeapFreeSpace(usedSpace.SizeIndex, usedSpace.EndIndex, FreeLinkedListNullPointer);


            BoundingFreeSpaces boundingSpaces = GetBoundingSpaces(token);

            LinkBefore(freedSpace, boundingSpaces.SpaceBefore);

            LinkAfter(freedSpace, boundingSpaces.SpaceAfter);

            PutFullSpace(freedSpace);

        }

        private void LinkBefore(PersistentHeapFreeSpace freedSpace, PersistentHeapFreeSpace before)
        {
            if (before == null) // there is no free space before the freed space
            {
                PutFreeSpaceHead(freedSpace.StartIndex);
            }
            else // there is at least one free space before the freed space
            {
                if (freedSpace.IsDirectlyAfter(before)) // the before space is next to freed space
                {
                    freedSpace.MergeBefore(before);
                }
                else // the before space is not next to freed space
                {
                    before.NextFreeSpace = freedSpace.StartIndex;
                    PutFullSpace(before);
                }
            }
        }

        private static void LinkAfter(PersistentHeapFreeSpace freedSpace, PersistentHeapFreeSpace after)
        {
            if (after == null) // there is no free space after the freed space
            {
                freedSpace.NextFreeSpace = FreeLinkedListNullPointer;
            }
            else // there is at least one free space after freed space
            {
                if (freedSpace.IsDirectlyBefore(after)) // the space after is next to freed space
                {
                    freedSpace.MergeAfter(after);
                }
                else // the space after is not next to freed space
                {
                    freedSpace.NextFreeSpace = after.StartIndex;
                }
            }
        }

        private class BoundingFreeSpaces
        {
            public PersistentHeapFreeSpace SpaceBefore { get; set; }
            public PersistentHeapFreeSpace SpaceAfter { get; set; }
        }

        private BoundingFreeSpaces GetBoundingSpaces(int token)
        {
            IEnumerator<PersistentHeapFreeSpace> freeSpaces = GetFreeSpaces().GetEnumerator();
            //get last free space before and first free space after
            PersistentHeapFreeSpace before = null;
            PersistentHeapFreeSpace after = null;
            if (freeSpaces.MoveNext()) // prime the pump
            {
                after = freeSpaces.Current; 
                while (freeSpaces.MoveNext() && after.StartIndex < token)
                {
                    before = after;
                    after = freeSpaces.Current;

                    if (after.SizeIndex == token) //the token passed in is already a free space
                        throw new InvalidIndexException("The space passed in is already free");

                }
            }
            if (after != null && after.StartIndex < token) // the freed space is at the end of the list (The loop ran out of spaces before getting a free space that is after freedSpace.
            {
                before = after;
                after = null;
            }

            freeSpaces.Dispose();

            return new BoundingFreeSpaces { SpaceAfter = after, SpaceBefore = before };
        }
        #endregion

        public byte[] Get(int index)
        {
            PersistentHeapUsedSpace space = GetUsedSpaceAt(index);
            return space.Data;
        }

        public void Put(int index, byte[] buffer)
        {
            PersistentHeapUsedSpace space = GetUsedSpaceAt(index);
            if(buffer.Length > space.UserSize)
                throw new IndexOutOfRangeException("The buffer is too big to fit");
            space.Data = buffer;
            PutFullSpace(space);
        }

        #region Get HeapSpaces
        private PersistentHeapUsedSpace GetSpaceAt(int token)
        {
            int sizeIndex = token - PersistentHeapSpace.GetUserSizeSize();
            byte[] sizeBytes = _file.GetAmount(sizeIndex, PersistentHeapSpace.GetUserSizeSize());
            int userSize = PersistentHeapSpace.GetUserSize(sizeBytes);

            int fullLength = userSize + PersistentHeapSpace.GetUserSizeSize();
            int endIndex = sizeIndex + fullLength-1;

            byte[] data = _file.GetRange(sizeIndex + PersistentHeapSpace.GetUserSizeSize(), endIndex);


            //let PersistentHeapUsedSpace be default
            PersistentHeapUsedSpace usedSpace = new PersistentHeapUsedSpace(sizeIndex, endIndex, data);
            return usedSpace;
        }

        private PersistentHeapUsedSpace GetUsedSpaceAt(int token)
        {
            return GetSpaceAt(token);
        }

        private PersistentHeapFreeSpace GetFreeSpaceAt(int token)
        {
            PersistentHeapUsedSpace usedSpace = GetSpaceAt(token);
            int next = usedSpace.Data.ToInt();

            PersistentHeapFreeSpace freeSpace = new PersistentHeapFreeSpace(usedSpace.SizeIndex, usedSpace.EndIndex, next);
            return freeSpace;
        }
        #endregion

        #region Put HeapSpaces
        private void PutFullSpace(PersistentHeapSpace space)
        {
            _file.Put(space.SizeIndex, space.Serialize());
        }

        private void PutSizeSpace(PersistentHeapSpace space)
        {
            _file.Put(space.StartIndex, space.UserSize.ToBytes());
        }
        #endregion

        private IEnumerable<PersistentHeapFreeSpace> GetFreeSpaces()
        {
            int freeSpaceHead = GetFreeSpaceHead();
            if (freeSpaceHead == FreeLinkedListNullPointer)
                yield break;
            PersistentHeapFreeSpace before = GetFreeSpaceAt(freeSpaceHead);
            Debug.Assert(before != null, "The check on the header's _freeSpaceHead should prevent this from being null");
            yield return before;

            while (before.NextFreeSpace != FreeLinkedListNullPointer)
            {
                before = GetFreeSpaceAt(before.NextFreeSpace);
                yield return before;
            }
        }

        #region IUserHeader
        public byte[] GetUserHeader()
        {
            byte[] fullHeader = _file.GetUserHeader();
            byte[] userHeader = fullHeader.SubArray(GetFreeSpaceHeadSize(), fullHeader.Length);
            return userHeader;
        }

        public int GetUserHeaderSize()
        {
            return _file.GetUserHeaderSize() - GetFreeSpaceHeadSize();
        }

        public void PutUserHeader(byte[] userHeader)
        {
            byte[] fullHeader = GetFreeSpaceHead().ToBytes().Append(userHeader);
            _file.PutUserHeader(fullHeader);
        }

        private void PutFreeSpaceHead(int freeSpaceHead)
        {
            byte[] newUserHeader = freeSpaceHead.ToBytes().Append(GetUserHeader());
            _file.PutUserHeader(newUserHeader);
        }

        private int GetFreeSpaceHead()
        {
            byte[] fullUserHeader = _file.GetUserHeader();
            byte[] freeSpaceHeadBytes = fullUserHeader.SubArray(0, GetFreeSpaceHeadSize());
            return freeSpaceHeadBytes.ToInt();
        }

        private int GetFreeSpaceHeadSize()
        {
            return PersistenceConstants.IntSize;
        }

        #endregion

        public void Close()
        {
            _file.Close();
        }

    }
}
