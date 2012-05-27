using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ByteHelper;
using ByteHelperUnitTest;
using Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PHT;

namespace PHTUnitTest
{
    [TestClass]
    public class HashTableHeaderTest
    {
        [TestMethod]
        public void CtorTest()
        {
            int tableSize = 20;
            int keySize = 5;
            int valueSize = 10;
            HashTableHeader header = new HashTableHeader(tableSize, keySize, valueSize);

            Assert.AreEqual(tableSize, header.TableSize);
            Assert.AreEqual(keySize, header.KeySize);
            Assert.AreEqual(valueSize, header.ValueSize);
        }

        [TestMethod]
        public void SerializeTest()
        {
            int tableSize = 20;
            int keySize = 5;
            int valueSize = 10;
            HashTableHeader header = new HashTableHeader(tableSize, keySize, valueSize);

            byte[] headerBytes = header.Serialize();
            byte[] expected = tableSize.ToBytes().Append(keySize.ToBytes(), valueSize.ToBytes());

            TestHelper.AssertByteArraysAreSame(expected, headerBytes);
        }

        [TestMethod]
        public void DeserializeTest()
        {
            int tableSize = 20;
            int keySize = 5;
            int valueSize = 10;

            byte[] expected = tableSize.ToBytes().Append(keySize.ToBytes(), valueSize.ToBytes());
            HashTableHeader header = HashTableHeader.Deserialize(expected);

            Assert.AreEqual(tableSize, header.TableSize);
            Assert.AreEqual(keySize, header.KeySize);
            Assert.AreEqual(valueSize, header.ValueSize);


            try
            {
                HashTableHeader.Deserialize(new byte[0]);
                Assert.Fail("Should throw exception");
            }
            catch (InsufficientDataException) { }
        }
    }
}
