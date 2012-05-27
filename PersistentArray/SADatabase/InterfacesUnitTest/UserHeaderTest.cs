using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace InterfacesUnitTest
{
    [TestClass]
    public abstract class UserHeaderTest
    {
        protected abstract IUserHeader GetIUserHeader(int userHeaderSize);

        /// <summary>
        ///A test for GetUserHeaderSize
        ///</summary>
        [TestMethod()]
        public void GetUserHeaderSizeTest()
        {
            GetUserHeaderSizeTestAssert(9);
            GetUserHeaderSizeTestAssert(1);
            GetUserHeaderSizeTestAssert(0);
            GetUserHeaderSizeTestAssert(1000000);
            try
            {
                GetUserHeaderSizeTestAssert(-1);
                Assert.Fail("Should throw exception");
            }
            catch (InvalidUserHeaderException) { }
        }

        private void GetUserHeaderSizeTestAssert(int expected)
        {
            IUserHeader target = GetIUserHeader(expected);
            try
            {
                int actual = target.GetUserHeaderSize();
                Assert.AreEqual(expected, actual);
            }
            finally
            {
                target.Close();
            }
        }
    }
}
