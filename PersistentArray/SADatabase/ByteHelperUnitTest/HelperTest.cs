using System;
using ByteHelper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ByteHelper.Exceptions;

namespace ByteHelperUnitTest
{
    [TestClass]
    public class HelperTest
    {

        /// <summary>
        ///A test for ToInt
        ///</summary>
        [TestMethod()]
        public void ToIntTest()
        {
            byte[] bytes = new byte[4] {100, 200, 32, 128};
            int expected = 1690837120; //http://www.exploringbinary.com/binary-converter/
            int actual = bytes.ToInt();

            Assert.AreEqual(expected, actual);

        }

        /// <summary>
        ///A test for ToBytes
        ///</summary>
        [TestMethod()]
        public void ToBytesTest()
        {
            int i = 1000000000;
            byte[] expected = new byte[4] { 59, 154, 202, 0 };
            byte[] actual = i.ToBytes();

            for (int j = 0; j < expected.Length; j++)
            {
                Assert.AreEqual(expected[j], actual[j]);
            }

        }

        /// <summary>
        ///A test for ToBytes
        ///</summary>
        [TestMethod()]
        public void GetHashCodeTest()
        {
            int hash = new byte[4].GetHash();
            Assert.AreEqual(1, hash);

            int hash2 = new byte[] {255, 255, 255, 255}.GetHash();
            Assert.AreEqual(1, hash2);
            
            int hash3 = new byte[] { 127,255,255,255 }.GetHash();
            Assert.AreEqual(int.MaxValue, hash3);

            int hash4 = new byte[] { 0,0,0,0 }.GetHash();
            Assert.AreEqual(1, hash4);
        }

        /// <summary>
        ///A test for CopyInto
        ///</summary>
        [TestMethod()]
        public void CopyIntoTest()
        {
            byte[] source = new byte[] {12, 34, 45, 56, 78, 90};
            byte[] destination1 = new byte[0];
            byte[] destination2 = new byte[5];
            byte[] destination3 = new byte[6];
            byte[] destination4 = new byte[7];
            byte[] destination5 = new byte[12];

            try {
                source.CopyInto(destination1);
                Assert.Fail("Should throw exception");
            }catch(IndexOutOfRangeException){}
            try {
                source.CopyInto(destination2);
                Assert.Fail("Should throw exception");
            }catch(IndexOutOfRangeException){}

            source.CopyInto(destination3);
            source.CopyInto(destination4);
            source.CopyInto(destination5);

            TestHelper.AssertByteArraysAreSame(source, destination3);
            TestHelper.AssertByteArraysAreSame(source, destination4);
            TestHelper.AssertByteArraysAreSame(source, destination5);
        }

        /// <summary>
        ///A test for Append1
        ///</summary>
        [TestMethod()]
        public void Append1Test()
        {
            byte[] dbl = new byte[] {1}.Append(new byte[] {2});
            TestHelper.AssertByteArraysAreSame(new byte[]{1,2}, dbl);

            byte[] empty = new byte[0].Append(new byte[0]);
            Assert.AreEqual(0, empty.Length);
        }

        /// <summary>
        ///A test for Append2
        ///</summary>
        [TestMethod()]
        public void Append2Test()
        {
            byte[] dbl = new byte[] { 1 }.Append(new byte[] { 2 }, new byte[]{3});
            TestHelper.AssertByteArraysAreSame(new byte[] { 1, 2,3 }, dbl);

            byte[] empty = new byte[0].Append(new byte[0], new byte[0]);
            Assert.AreEqual(0, empty.Length);
        }

        /// <summary>
        ///A test for ExtendTo
        ///</summary>
        [TestMethod()]
        public void ExtendToTest()
        {

            byte[] dbl = new byte[] {1};
            int dblExtendtToLength = 3;
            byte[] extendedDbl = dbl.ExtendTo(dblExtendtToLength);
            Assert.AreEqual(dblExtendtToLength, extendedDbl.Length);
            TestHelper.AssertByteArraysAreSame(new byte[] { 1, 0, 0 }, extendedDbl);

            byte[] empty = new byte[0];
            int emptyExtendtToLength = 3;
            byte[] extendEmpty = empty.ExtendTo(emptyExtendtToLength);
            Assert.AreEqual(emptyExtendtToLength, extendEmpty.Length);
            TestHelper.AssertByteArraysAreSame(new byte[] { 0, 0, 0 }, extendEmpty);

            byte[] same = new byte[] {1, 2, 3, 4};
            int sameExtendtToLength = 4;
            byte[] extendedSame = same.ExtendTo(sameExtendtToLength);
            Assert.AreEqual(sameExtendtToLength, extendedSame.Length);
            TestHelper.AssertByteArraysAreSame(new byte[] { 1, 2, 3, 4 }, extendedSame);

            try{
                new byte[6].ExtendTo(0);
                Assert.Fail("Should throw exception");
            }
            catch (SizeTooSmallException) { }

            try{
                new byte[6].ExtendTo(5);
                Assert.Fail("Should throw exception");
            }
            catch (SizeTooSmallException) { }
        }

        /// <summary>
        ///A test for SubArray
        ///</summary>
        [TestMethod()]
        public void SubArrayTest()
        {

            byte[] dbl = new byte[] { 1, 2, 3, 4 };
            byte[] dblActual = dbl.SubArray(1,2);
            TestHelper.AssertByteArraysAreSame(new byte[] { 2 }, dblActual);

            byte[] all = new byte[] {4, 5, 6};
            byte[] allActual = all.SubArray(0, 3);
            TestHelper.AssertByteArraysAreSame(new byte[] { 4,5,6 }, allActual);

            byte[] same = new byte[] { 1, 2, 3, 4 };
            byte[] sameActual = same.SubArray(1, 4);
            TestHelper.AssertByteArraysAreSame(new byte[] { 2, 3, 4 }, sameActual);

            byte[] equal = new byte[] { 1, 2, 3, 4 };
            byte[] equalActual = equal.SubArray(2,2);
            TestHelper.AssertByteArraysAreSame(new byte[0], equalActual);

            try
            {
                new byte[9].SubArray(3, 2);
                Assert.Fail("Should throw exception");
            }
            catch (InvalidRangeException) { }

            try
            {
                new byte[5].SubArray(7, 8);
                Assert.Fail("Should throw exception");
            }
            catch (IndexOutOfRangeException) { }

            try
            {
                new byte[4].SubArray(3, 9);
                Assert.Fail("Should throw exception");
            }
            catch (IndexOutOfRangeException) { }
        }

        /// <summary>
        ///A test for Copy
        ///</summary>
        [TestMethod()]
        public void CopyTest()
        {
            CopyTestAssert(new byte[] {1});
            CopyTestAssert(new byte[] { 1,2,3,4,5,6,7,8,9,0 });
            CopyTestAssert(new byte[] { byte.MaxValue, byte.MinValue });
            CopyTestAssert(new byte[] { 0 });
            CopyTestAssert(new byte[300]);
        }

        private static void CopyTestAssert(byte[] source)
        {
            byte[] copy = source.Copy();
            TestHelper.AssertByteArraysAreSame(source, copy);
        }

        /// <summary>
        ///A test for EqualsBytes
        ///</summary>
        [TestMethod()]
        public void EqualsBytesTest()
        {
            Assert.IsTrue(new byte[] { 0 }.EqualsBytes(new byte[] { 0 }));
            Assert.IsTrue(new byte[] { 1, 2, 3, 4 }.EqualsBytes(new byte[] { 1, 2, 3, 4 }));
            Assert.IsTrue(new byte[] { byte.MaxValue }.EqualsBytes(new byte[] { byte.MaxValue }));
            Assert.IsTrue(new byte[] { byte.MinValue }.EqualsBytes(new byte[] { byte.MinValue }));
            Assert.IsTrue(new byte[] { 100, 200 }.EqualsBytes(new byte[] { 100, 200 }));
            Assert.IsTrue(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 }.EqualsBytes(new byte[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30 }));
        }
    }
}
