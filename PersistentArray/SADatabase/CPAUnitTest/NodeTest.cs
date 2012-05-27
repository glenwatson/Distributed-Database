using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using CPA.Cache.LinkedList;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PA;

namespace CPAUnitTest
{
    /// <summary>
    /// Summary description for NodeTest
    /// </summary>
    [TestClass]
    public class NodeTest
    {
        public NodeTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        [TestMethod]
        public void NodeCtorTest()
        {
            Element ele = new Element(new byte[0]);
            int elementIdx = 0;
            Node parent = new Node(null, null, ele, elementIdx);
            Node child = new Node(null, null, ele, elementIdx);
            NodeCtorTestAssert(elementIdx, ele, parent, child);

        }

        private static void NodeCtorTestAssert(int elementIdx, Element ele, Node parent, Node child)
        {
            Node n = new Node(child, parent, ele, elementIdx);

            Assert.AreEqual(n.Element, ele);
            Assert.AreEqual(n.ElementIndex, elementIdx);
            Assert.AreEqual(n.Child, child);
            Assert.AreEqual(n.Parent, parent);
        }

        [TestMethod]
        public void SetChildTest()
        {
            Node child = new Node(null, null, new Element(new byte[0]), 0);

            Node n = new Node(null, null, new Element(new byte[0]), 1);

            n.SetChild(child);

            Assert.AreEqual(child, n.Child);
        }

        [TestMethod]
        public void SetParentTest()
        {
            Node parent = new Node(null, null, new Element(new byte[0]), 0);

            Node n = new Node(null, null, new Element(new byte[0]), 1);

            n.SetParent(parent);

            Assert.AreEqual(parent, n.Parent);
        }
    }
}
