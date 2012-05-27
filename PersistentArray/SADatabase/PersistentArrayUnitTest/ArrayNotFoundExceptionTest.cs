//using PA.Exceptions;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace PAUnitTest
//{
    
    
//    /// <summary>
//    ///This is a test class for ArrayNotFoundExceptionTest and is intended
//    ///to contain all ArrayNotFoundExceptionTest Unit Tests
//    ///</summary>
//    [TestClass()]
//    public class ArrayNotFoundExceptionTest
//    {


//        private TestContext testContextInstance;

//        /// <summary>
//        ///Gets or sets the test context which provides
//        ///information about and functionality for the current test run.
//        ///</summary>
//        public TestContext TestContext
//        {
//            get
//            {
//                return testContextInstance;
//            }
//            set
//            {
//                testContextInstance = value;
//            }
//        }

//        #region Additional test attributes
//        // 
//        //You can use the following additional attributes as you write your tests:
//        //
//        //Use ClassInitialize to run code before running the first test in the class
//        //[ClassInitialize()]
//        //public static void MyClassInitialize(TestContext testContext)
//        //{
//        //}
//        //
//        //Use ClassCleanup to run code after all tests in a class have run
//        //[ClassCleanup()]
//        //public static void MyClassCleanup()
//        //{
//        //}
//        //
//        //Use TestInitialize to run code before running each test
//        //[TestInitialize()]
//        //public void MyTestInitialize()
//        //{
//        //}
//        //
//        //Use TestCleanup to run code after each test has run
//        //[TestCleanup()]
//        //public void MyTestCleanup()
//        //{
//        //}
//        //
//        #endregion


//        /// <summary>
//        ///A test for ArrayNotFoundException Constructor
//        ///</summary>
//        [TestMethod()]
//        public void ArrayNotFoundExceptionConstructorTest()
//        {
//            string arrayName = "arrayName";
//            ArrayNotFoundException target = new ArrayNotFoundException(arrayName);
//            Assert.IsNotNull(target);
//        }

//        /// <summary>
//        ///A test for ArrayName
//        ///</summary>
//        [TestMethod()]
//        public void ArrayNameTest()
//        {
//            PrivateObject param0 = new PrivateObject(new ArrayNotFoundException(""));
//            ArrayNotFoundException_Accessor target = new ArrayNotFoundException_Accessor(param0);
//            string expected = "arrayName";
//            string actual;
//            target.ArrayName = expected;
//            actual = target.ArrayName;
//            Assert.AreEqual(expected, actual);
//        }
//    }
//}
