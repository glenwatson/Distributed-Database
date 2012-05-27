using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using CPA.Cache.LinkedList;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PA;
using PA.Exceptions;

namespace CPAUnitTest
{
    [TestClass]
    public class LRUDoubleLinkedListTest
    {
        [TestMethod]
        public void LRUDoublyLinkedListCtorTest()
        {
            LRUDoublyLinkedList dll = new LRUDoublyLinkedList();
            Assert.IsNotNull(dll);
        }

        [TestMethod]
        public void GetTest()
        {
            LRUDoublyLinkedList dll = new LRUDoublyLinkedList();
            Assert.IsNull(dll.Get(0));
            Assert.IsNull(dll.Get(5));
            try
            {
                dll.Get(-1);
                Assert.Fail("Should throw an exception");
            }
            catch (InvalidElementIndexException) {}

            Node n0 = new Node(null, null, new Element(new byte[0]), 0);
            dll.Insert(n0);
            Assert.AreEqual(dll.Get(0), n0);
            Assert.IsNull(dll.Get(1));
            Assert.IsNull(dll.Get(2));

            Node n1 = new Node(null, null, new Element(new byte[0]), 1);
            dll.Insert(n1);
            Assert.AreEqual(dll.Get(0), n1);
            Assert.AreEqual(dll.Get(1), n0);
            Assert.AreEqual(dll.Get(0), n0);
            Assert.IsNull(dll.Get(2));
        }

        [TestMethod]
        public void InsertTest()
        {
            InsertOneTest();

            InsertMoreThanOneTest();

            InsertMoreThanTwoTest();
        }

        private void InsertOneTest()
        {
            LRUDoublyLinkedList dll = new LRUDoublyLinkedList();

            Node n0 = new Node(null, null, new Element(new byte[0]), 0);
            Assert.AreEqual(0, dll.Size);
            dll.Insert(n0);
            Assert.AreEqual(n0, dll.Get(0));
            Assert.AreEqual(1, dll.Size);
        }

        private void InsertMoreThanOneTest()
        {
            LRUDoublyLinkedList dll = new LRUDoublyLinkedList();
            Node n0 = new Node(null, null, new Element(new byte[0]), 0);
            dll.Insert(n0);
            Node n1 = new Node(null, null, new Element(new byte[0]), 1);
            dll.Insert(n1);
            Assert.AreEqual(n1, dll.Get(0));
            Assert.AreEqual(n0, dll.Get(1));
            Assert.AreEqual(2, dll.Size);
        }

        private void InsertMoreThanTwoTest()
        {
            LRUDoublyLinkedList dll = new LRUDoublyLinkedList();

            Node n0 = new Node(null, null, new Element(new byte[0]), 0);
            dll.Insert(n0);
            Node n1 = new Node(null, null, new Element(new byte[0]), 1);
            dll.Insert(n1);
            Node n2 = new Node(null, null, new Element(new byte[0]), 2);
            dll.Insert(n2);

            Assert.AreEqual(n2, dll.Get(0));
            Assert.AreEqual(n1, dll.Get(1));
            Assert.AreEqual(n0, dll.Get(2));
            Assert.AreEqual(3, dll.Size);
        }

        [TestMethod]
        public void MoveToHeadTest()
        {
            LRUDoublyLinkedList dll = new LRUDoublyLinkedList();

            Node n0 = new Node(null, null, new Element(new byte[0]), 0);
            dll.Insert(n0);
            dll.MoveToHead(n0);
            Assert.AreEqual(n0, dll.Get(0));
            Assert.AreEqual(1, dll.Size);


            Node n1 = new Node(null, null, new Element(new byte[0]), 1);
            dll.Insert(n1);
            dll.MoveToHead(n0);
            Assert.AreEqual(n0, dll.Get(0));
            Assert.AreEqual(2, dll.Size);

            Node n2 = new Node(null, null, new Element(new byte[0]), 2);
            dll.Insert(n2);
            dll.MoveToHead(n0);
            Assert.AreEqual(n0, dll.Get(0));
            Assert.AreEqual(3, dll.Size);
        }

        [TestMethod]
        public void RemoveEndTest()
        {
            LRUDoublyLinkedList dll = new LRUDoublyLinkedList();

            Assert.IsNull(dll.RemoveEnd());

            Node n0 = new Node(null, null, new Element(new byte[0]), 0);
            dll.Insert(n0);
            dll.RemoveEnd();
            Assert.IsNull(dll.Get(0));
            Assert.AreEqual(0, dll.Size);

            dll.Insert(n0);
            Node n1 = new Node(null, null, new Element(new byte[0]), 1);
            dll.Insert(n1);
            dll.RemoveEnd();

            Assert.AreEqual(dll.Get(0), n1);
            Assert.IsNull(dll.Get(1));
            Assert.AreEqual(1, dll.Size);
        }
    }
}
