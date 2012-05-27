using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using ByteHelper;
using ByteHelperUnitTest;
using FilePersistence.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PHT;
using PHT.Exceptions;

namespace PHTUnitTest
{
    [TestClass]
    public class PersistentHashTableTest
    {
        private PersistentHashTable InitTable(string hashtableName, int tableSize, int keySize, int valueSize, int userHeaderSize)
        {
            PersistentHashTable ht;
            try
            {
                ht = new PersistentHashTable(hashtableName, tableSize, keySize, valueSize, userHeaderSize);
            }
            catch (FileNameConflictException)
            {
                ht = new PersistentHashTable(hashtableName);
                ht.Delete();
                ht = new PersistentHashTable(hashtableName, tableSize, keySize, valueSize, userHeaderSize);
            }
            return ht;
        }

        [TestMethod]
        public void CtorTest()
        {
            const int tableSize = 88;
            const int keySize = 3;
            const int valueSize = 10;
            PersistentHashTable hashTable = InitTable("ctor", tableSize, keySize, valueSize, 4);
            try
            {
                Assert.AreEqual(tableSize, hashTable.GetTableSize());
                Assert.AreEqual(keySize, hashTable.GetKeySize());
                Assert.AreEqual(valueSize, hashTable.GetValueSize());
            }
            finally
            {
                hashTable.Close();
            }
        }

        [TestMethod]
        public void ReopenTest()
        {
            string hastTableName = "ReopenHashTableTest";
            PersistentHashTable hashTable = InitTable(hastTableName, 20, 4, 4, 6);
            try
            {
                byte[] key = new byte[] {4, 4, 4, 4};
                byte[] value = new byte[] { 5,5,5,5 };
                hashTable.Put(key, value);
                hashTable.Close();

                hashTable = new PersistentHashTable(hastTableName);

                TestHelper.AssertByteArraysAreSame(value, hashTable.Get(key));
            }
            finally
            {
                hashTable.Close();
            }
        }

        [TestMethod]
        public void PutGetTest()
        {
            const int keySize = 5;
            const int valueSize = 10;
            PersistentHashTable hashTable = InitTable("PutGetHashTable", 15, keySize, valueSize, 7);
            try
            {
                PutGetTestAssert(hashTable, new byte[keySize] { 1, 2, 3, 4, 5 }, new byte[valueSize] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 });
                PutGetTestAssert(hashTable, new byte[keySize] { byte.MinValue, byte.MaxValue, 0, 0, 0 }, new byte[valueSize] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 });
                PutGetTestAssert(hashTable, new byte[keySize] { 128,64,32,16,8 }, new byte[valueSize] { byte.MinValue, byte.MinValue, 0,0,0,0,0,0,0,0 });

                try
                {
                    hashTable.Put(new byte[0], new byte[valueSize]);
                    Assert.Fail("Should throw exception");
                } catch (InvalidKeySizeException) {}

                try
                {
                    hashTable.Put(new byte[1], new byte[valueSize]);
                    Assert.Fail("Should throw exception");
                }catch (InvalidKeySizeException) { }

                try
                {
                    hashTable.Put(new byte[keySize]{1,2,3,4,5}, new byte[0]);
                    Assert.Fail("Should throw exception");
                } catch (InvalidValueSizeException) { }

                try
                {
                    hashTable.Put(new byte[keySize]{1,2,3,4,5}, new byte[1]);
                    Assert.Fail("Should throw exception");
                }
                catch (InvalidValueSizeException) { }

                try
                {
                    hashTable.Put(new byte[keySize], new byte[valueSize]);
                    Assert.Fail("Should throw exception");
                }catch (InvalidKeyException) { }
            }
            finally
            {
                hashTable.Close();
            }
        }

        private static void PutGetTestAssert(PersistentHashTable hashTable, byte[] key, byte[] value)
        {
            hashTable.Put(key, value);
            byte[] actual = hashTable.Get(key);
            TestHelper.AssertByteArraysAreSame(value, actual);
        }

        [TestMethod]
        public void RemoveTest()
        {
            const int keySize = 4;
            const int valueSize = 4;
            PersistentHashTable hashTable = InitTable("HashTableRemove", 15, keySize, valueSize, 6);
            try
            {
                RemoveTestAssert(hashTable, valueSize, new byte[keySize] { 1, 2, 3, 4 });
                RemoveTestAssert(hashTable, valueSize, new byte[keySize] { 1, 0, 0, 0 });
                RemoveTestAssert(hashTable, valueSize, new byte[keySize] { 0,0,0,1 });
                RemoveTestAssert(hashTable, valueSize, new byte[keySize] { byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue });
            }
            finally
            {
                hashTable.Close();
            }
        }

        private static void RemoveTestAssert(PersistentHashTable hashTable, int valueSize, byte[] key)
        {
            hashTable.Put(key, new byte[valueSize]);
            hashTable.Remove(key);
            try
            {
                hashTable.Get(key);
            }
            catch (KeyNotFoundException){}
        }

        [TestMethod]
        public void CloseTest()
        {
            PersistentHashTable hashTable = InitTable("Close", 88, 3, 10, 6);
            try
            {
                hashTable.Close();
            }
            finally
            {
                hashTable.Close();
            }
        }

        [TestMethod]
        public void PutGetUserHeaderTest()
        {
            const string tableName = "PutGetHashTableUserHeader";
            const int tableSize = 88;
            const int keySize = 3;
            const int valueSize = 10;
            const int userHeaderSize = 6;
            PersistentHashTable hashTable = InitTable(tableName, tableSize, keySize, valueSize, userHeaderSize);
            try
            {
                byte[] newUserHeader = new byte[userHeaderSize]{1,2,3,4,5,6};
                hashTable.PutUserHeader(newUserHeader);
                byte[] actual = hashTable.GetUserHeader();

                TestHelper.AssertByteArraysAreSame(newUserHeader, actual);

            }
            finally
            {
                hashTable.Close();
            }
        }

        [TestMethod]
        public void GetUserHeaderSizeTest()
        {
            const int userHeaderSize = 6;
            PersistentHashTable hashTable = InitTable("GetHashTableUserHeaderSize", 3, 2, 2, userHeaderSize);
            try
            {
                Assert.AreEqual(userHeaderSize, hashTable.GetUserHeaderSize());
            }
            finally
            {
                hashTable.Close();
            }
        }


        [TestMethod]
        public void FillUpTableTest()
        {
            const string tableName = "FillUpHashTable";
            const int tableSize = 25;
            const int keySize = 4;
            const int valueSize = 4;
            const int userHeaderSize = 6;
            PersistentHashTable hashTable = InitTable(tableName, tableSize, keySize, valueSize, userHeaderSize);
            try
            {
                // fill the hash table
                for (int i = 1; i < tableSize+1; i++)
                {
                    hashTable.Put(i.ToBytes(), i.ToBytes());
                }

                //add one more than allowed
                try
                {
                    hashTable.Put(new byte[] { byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue }, new byte[] { byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue });
                    Assert.Fail("Should throw exception");
                }
                catch (IndexOutOfRangeException){}
                
            }
            finally
            {
                hashTable.Close();
            }
        }

        [TestMethod]
        public void PutGetCollisionsTest()
        {
            const string tableName = "HashTablePutCollisions";
            const int tableSize = 20;
            const int keySize = 4;
            const int valueSize = 4;
            const int userHeaderSize = 6;
            PersistentHashTable hashTable = InitTable(tableName, tableSize, keySize, valueSize, userHeaderSize);
            try
            {
                //fill the table up with collisions
                byte[] lastCollisionKey = FillTableWithCollisions(5, hashTable);

                TestHelper.AssertByteArraysAreSame(new byte[] { 0, 0, 0, tableSize-1 }, hashTable.Get(lastCollisionKey));

            }
            finally
            {
                hashTable.Close();
            }
        }

        [TestMethod]
        public void RemoveCollisionTest()
        {
            PersistentHashTable hashTable = InitTable("HashTableRemove", 20, 4, 4, 6);
            try
            {
                //fill the table up with collisions
                byte[] lastCollisionKey = FillTableWithCollisions(5, hashTable);

                hashTable.Remove(lastCollisionKey);

                hashTable.Put(lastCollisionKey, new byte[hashTable.GetValueSize()]);
            }
            finally
            {
                hashTable.Close();
            }
        }

        private static byte[] FillTableWithCollisions(int targetHashToCollideOn, PersistentHashTable hashTable)
        {
            byte[] collisionKey = new byte[] {0, 0, 0, 0};
            for (int run = 0; run < hashTable.GetTableSize(); run++)
            {
                collisionKey = GetNextCollisionKey(targetHashToCollideOn, hashTable.GetTableSize(), collisionKey);

                hashTable.Put(collisionKey, new byte[] {0, 0, 0, (byte) run});
            }

            //assert the table is full
            try
            {
                hashTable.Put(new byte[] { 255, 255, 255, 255 }, new byte[hashTable.GetValueSize()]);
                Assert.Fail("Should throw exception");
            }
            catch (IndexOutOfRangeException) {}
            return collisionKey;
        }

        private static byte[] GetNextCollisionKey(int targetHashToCollideOn, int tableSize, byte[] collisionKey)
        {
            // generate collision
            do {
                collisionKey = (collisionKey.ToInt() + 1).ToBytes();
            } while (collisionKey.GetHash()%tableSize != targetHashToCollideOn);
            return collisionKey;
        }
    }
}
