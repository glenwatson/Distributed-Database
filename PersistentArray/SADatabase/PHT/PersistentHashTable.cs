using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ByteHelper;
using Interfaces;
using PA;
using PA.Exceptions;
using PHT.Exceptions;

namespace PHT
{
    public class PersistentHashTable : IHashTable
    {
        private readonly IPersistentArrayNextSpace _simpleCollectionNextIndex;
        private byte[] NullKey {get {return new byte[GetKeySize()];}}

        public PersistentHashTable(string hashtableName, int tableSize, int keySize, int valueSize, int userHeaderSize)
        {
            _simpleCollectionNextIndex = new PersistentNextSpaceArray(hashtableName, keySize + valueSize, userHeaderSize + GetHeaderSize());
            HashTableHeader header = new HashTableHeader(tableSize, keySize, valueSize);
            _simpleCollectionNextIndex.PutUserHeader(header.Serialize());
            _simpleCollectionNextIndex.Put(tableSize-1, new byte[0]);
        }

        public PersistentHashTable(string hashtableName)
        {
            _simpleCollectionNextIndex = new PersistentNextSpaceArray(hashtableName);
        }

        public int GetTableSize()
        {
            return GetHashTableHeader().TableSize;
        }

        public int GetKeySize()
        {
            return GetHashTableHeader().KeySize;
        }

        public int GetValueSize()
        {
            return GetHashTableHeader().ValueSize;
        }

        public void Delete()
        {
            _simpleCollectionNextIndex.Delete();
        }

        #region User/Header
        private HashTableHeader GetHashTableHeader()
        {
            byte[] fullHeader = _simpleCollectionNextIndex.GetUserHeader();
            byte[] myHeader = fullHeader.SubArray(0, GetHeaderSize());
            return HashTableHeader.Deserialize(myHeader);
        }

        public static int GetHeaderSize()
        {
            return HashTableHeader.GetHeaderSize();
        }

        public byte[] GetUserHeader()
        {
            byte[] fullHeader = _simpleCollectionNextIndex.GetUserHeader();
            byte[] userHeader = fullHeader.SubArray(GetHeaderSize(), fullHeader.Length);
            return userHeader;
        }

        public int GetUserHeaderSize()
        {
            return _simpleCollectionNextIndex.GetUserHeaderSize() - GetHeaderSize();
        }

        public void PutUserHeader(byte[] userHeader)
        {
            byte[] headerBytes = GetHashTableHeader().Serialize();
            byte[] newUserHeader = headerBytes.Append(userHeader);
            _simpleCollectionNextIndex.PutUserHeader(newUserHeader);
        }

        #endregion

        public void Close()
        {
            _simpleCollectionNextIndex.Close();
        }

        public void Put(byte[] key, byte[] value)
        {
            AssertValidKey(key);
            AssertValidValue(value);
            int putIndex = GetNextAvailableSpaceForKey(key).Index;
            HashTableElement element = new HashTableElement(key, value, putIndex);
            PutElement(element);
        }

        private void PutElement(HashTableElement element)
        {
            _simpleCollectionNextIndex.Put(element.Index, element.Key.Append(element.Value));
        }

        #region Get

        private HashTableElement GetNextAvailableSpaceForKey(byte[] key)
        {
            return GetNextAvailableSpaceForKeyEnumerable(key).Last();
        }

        private HashTableElement GetElementWithKey(byte[] key)
        {
            return GetElementWithKeyEnumerable(key).Last();
        }

        private HashTableElement GetElementAt(int index)
        {
            byte[] getResult = _simpleCollectionNextIndex.Get(index);
            HashTableHeader header = GetHashTableHeader();
            return HashTableElement.Deserialize(getResult, index, header.KeySize, header.ValueSize);
        }

        private int GetNextIndexAfterCollision(int hash)
        {
            int nextIndex = hash + 1;
            int tableSize = GetTableSize();
            if (nextIndex >= tableSize) //wrap around the array
                nextIndex = nextIndex - tableSize;
            return nextIndex;
        }

        public byte[] Get(byte[] key)
        {
            AssertValidKey(key);
            return GetElementWithKey(key).Value;
        }

        #endregion

        public void Remove(byte[] key)
        {
            AssertValidKey(key);
            //find the element while adding to moveCandidates
            SortedSet<HashTableElement> moveCandidates = new SortedSet<HashTableElement>();
            HashTableElement removingElement = null;
            foreach (var hashTableElement in GetElementWithKeyEnumerable(key))
            {
                Debug.Assert(moveCandidates.Add(hashTableElement));
                removingElement = hashTableElement;
            }
            Debug.Assert(removingElement != null, "GetElementWithKeyEnumberable() should have returned at least one element or thrown an exception");

            moveCandidates.Remove(removingElement);

            //null out the element
            WipeArrayAt(removingElement.Index);

            //rehash the elements between the hash and the removed element
            foreach (var moveCandidate in moveCandidates)
            {
                Debug.Assert(!moveCandidate.Key.EqualsBytes(NullKey), "GetElementWithKeyEnumerable() should not return any null elements");
                WipeArrayAt(moveCandidate.Index);
                Put(moveCandidate.Key, moveCandidate.Value);
            }
        }

        //private class HashTableElementComparer : IComparer<HashTableElement>
        //{
        //    public int Compare(HashTableElement x, HashTableElement y)
        //    {
        //        return x.Key.GetHash() - y.Key.GetHash();
        //    }
        //}

        private void WipeArrayAt(int index)
        {
            HashTableElement element = new HashTableElement(NullKey, new byte[GetValueSize()], index);
            PutElement(element);
        }

        private void AssertValidKey(byte[] key)
        {
            if (key.EqualsBytes(NullKey))
                throw new InvalidKeyException("Key cannot be all zeros");
            int keySize = GetKeySize();
            if(key.Length != keySize)
                throw new InvalidKeySizeException("Key has to be "+keySize+" bytes long");
        }

        private void AssertValidValue(byte[] value)
        {
            int valueSize = GetValueSize();
            if (value.Length != valueSize)
                throw new InvalidValueSizeException("value must be " + valueSize + " bytes long");
        }

        #region I am brilliant

        private IEnumerable<HashTableElement> GetElementWithKeyEnumerable(byte[] key)
        {
            int hash = key.GetHash();
            int nextAvailableArrayIndex = hash % GetTableSize();
            HashTableElement element = GetElementAt(nextAvailableArrayIndex);
            yield return element;
            while (!element.Key.EqualsBytes(key) && !element.Key.EqualsBytes(NullKey))
            {
                int nextIndex = GetNextIndexAfterCollision(element.Index);
                while (nextIndex >= _simpleCollectionNextIndex.GetNextIndex()) //there's nothing past this so simulate a wrap around
                    nextIndex = GetNextIndexAfterCollision(element.Index);
                element = GetElementAt(nextIndex);
                //if (element.Key.EqualsBytes(key)) // we've looped all the way around
                //    throw new KeyNotFoundException("key not found");
                yield return element;
            }
            if (element.Key.EqualsBytes(NullKey)) // key was not located before the null key was found
                throw new KeyNotFoundException("key not found");
        }


        private IEnumerable<HashTableElement> GetNextAvailableSpaceForKeyEnumerable(byte[] key)
        {
            int hash = key.GetHash();
            int initialIndexForKey = hash % GetTableSize();
            HashTableElement element = GetElementAt(initialIndexForKey);
            yield return element;
            while (!element.Key.EqualsBytes(NullKey) && element.Index < _simpleCollectionNextIndex.GetNextIndex()) //collision and still within the array's boundries
            {
                int nextIndex = GetNextIndexAfterCollision(element.Index);
                element = GetElementAt(nextIndex);
                if(element.Key.EqualsBytes(key)) //duplicate key
                    throw new InvalidKeyException("That key already exsists in the table");
                if (element.Index == initialIndexForKey) // we've looped all the way around
                    throw new IndexOutOfRangeException("The hash table is full");
                yield return element;
            }
            if (element.Index >= _simpleCollectionNextIndex.GetNextIndex())
                element = new HashTableElement(key, new byte[GetValueSize()], element.Index);

            yield return element;
        }
        #endregion

    }
}
