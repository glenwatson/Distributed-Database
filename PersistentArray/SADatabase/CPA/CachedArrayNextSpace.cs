using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ByteHelper;
using CPA.Cache;
using Interfaces;
using PA;
using Persistence;

namespace CPA
{
    public class CachedArray : IPersistentArrayNextSpace
    {
        private readonly IPersistentArrayNextSpace _simpleCollectionNextSpace;
        private LRUElementCache _cache;

        public CachedArray(string arrayName, int elementSize, int userHeaderSize, int cacheSize)
        {
            _simpleCollectionNextSpace = new PersistentNextSpaceArray(arrayName, elementSize, userHeaderSize + GetCacheSizeSize());
            _cache = new LRUElementCache(cacheSize);
            PutUserHeader(new byte[0]);
        }

        public CachedArray(string arrayName)
        {
            _simpleCollectionNextSpace = new PersistentNextSpaceArray(arrayName);
            InitCache();
        }

        private void InitCache()
        {
            int cacheSize = InitCacheSize();
            _cache = new LRUElementCache(cacheSize);
        }

        private int InitCacheSize()
        {
            byte[] fullUserHeaderBytes = _simpleCollectionNextSpace.GetUserHeader();
            byte[] cacheSizeBytes = fullUserHeaderBytes.SubArray(0, GetCacheSizeSize());
            return cacheSizeBytes.ToInt();
        }
        
        public byte[] Get(int index)
        {
            Element element = _cache.Get(index);
            byte[] result;
            if (element == null)
            {
                result = _simpleCollectionNextSpace.Get(index);
            }
            else
            {
                result = element.Data;
            }

            return result;
        }

        public void Put(int index, byte[] buffer)
        {
            int elementSize = GetElementSize();
            byte[] extendedBuffer = buffer.ExtendTo(elementSize);
            _simpleCollectionNextSpace.Put(index, extendedBuffer);
            _cache.AddToCache(new Element(extendedBuffer), index);
        }

        public void Delete()
        {
            _simpleCollectionNextSpace.Delete();
        }

        #region UserHeader
        public int GetUserHeaderSize()
        {
            return _simpleCollectionNextSpace.GetUserHeaderSize() - GetCacheSizeSize();
        }

        public byte[] GetUserHeader()
        {
            byte[] fullUserHeaderBytes = _simpleCollectionNextSpace.GetUserHeader();
            return fullUserHeaderBytes.SubArray(GetCacheSizeSize(), fullUserHeaderBytes.Length);
        }

        public void PutUserHeader(byte[] userHeader)
        {
            byte[] userHeaderBytes = _cache.Size.ToBytes().Append(userHeader);
            _simpleCollectionNextSpace.PutUserHeader(userHeaderBytes);
        }

        private static int GetCacheSizeSize()
        {
            return PersistenceConstants.IntSize;
        }
        #endregion

        #region Proxy
        public void Close()
        {
            _simpleCollectionNextSpace.Close();
        }

        public int GetElementSize()
        {
            return _simpleCollectionNextSpace.GetElementSize();
        }

        public int GetNextIndex()
        {
            return _simpleCollectionNextSpace.GetNextIndex();
        }

        public void WipeElement(int index)
        {
            _cache.AddToCache(new Element(new byte[_simpleCollectionNextSpace.GetElementSize()]), index);
            _simpleCollectionNextSpace.WipeElement(index);
        }
        #endregion

        public int GetCacheSize()
        {
            return _cache.Size;
        }
    }
}
