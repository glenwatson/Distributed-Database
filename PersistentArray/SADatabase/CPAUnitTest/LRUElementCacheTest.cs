using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using CPA.Cache;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PA;

namespace CPAUnitTest
{
    [TestClass]
    public class LRUElementCacheTest
    {
        [TestMethod]
        public void LRUElementCacheCtorTest()
        {
            LRUElementCache cache = new LRUElementCache(9);
            Assert.IsNotNull(cache);
        }

        [TestMethod]
        public void GetTest()
        {
            LRUElementCache cache = new LRUElementCache(9);

            int index = 0;
            Element ele = new Element(new byte[0]);
            GetAddTestAssert(index, ele, cache);

            int index1 = 1;
            Element ele1 = new Element(new byte[0]);
            GetAddTestAssert(index1, ele1, cache);

            int index2 = 2;
            Element ele2 = new Element(new byte[0]);
            GetAddTestAssert(index2, ele2, cache);

            int index3 = 2;
            Element ele3 = new Element(new byte[0]);
            GetAddTestAssert(index3, ele3, cache);

            Element ele4 = cache.Get(4);
            Assert.IsNull(ele4);

            LRUElementCache emptyCache = new LRUElementCache(0);
            emptyCache.AddToCache(new Element(new byte[0]), 0);
            Assert.IsNull(emptyCache.Get(0));

            LRUElementCache smallCache = new LRUElementCache(1);
            smallCache.AddToCache(new Element(new byte[0]), 0);
            smallCache.AddToCache(new Element(new byte[0]), 7);
            Assert.IsNotNull(smallCache.Get(7));
            Assert.IsNull(smallCache.Get(0));

        }

        private static void GetAddTestAssert(int index, Element ele, LRUElementCache cache)
        {
            cache.AddToCache(ele, index);

            Element actual = cache.Get(index);

            Assert.AreEqual(ele, actual);
        }
    }
}
