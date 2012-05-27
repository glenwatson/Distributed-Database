using FilePersistence;
using Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FilePersistenceUnitTest
{
    [TestClass]
    public class StorageMultitonTest
    {
        [TestMethod]
        public void SingletonTest()
        {
            string name = "singleton";
            IStorage storage1 = StorageMultiton.GetInstance(name);
            IStorage storage2 = StorageMultiton.GetInstance(name);
            Assert.AreNotEqual(storage1, storage2);
        }

        [TestMethod]
        public void MultitonTest()
        {
            string name1 = "one";
            string name2 = "two";

            IStorage storage11 = StorageMultiton.GetInstance(name1);
            IStorage storage12 = StorageMultiton.GetInstance(name1);
            Assert.AreNotEqual(storage11, storage12);

            IStorage storage21 = StorageMultiton.GetInstance(name2);
            IStorage storage22 = StorageMultiton.GetInstance(name2);
            Assert.AreNotEqual(storage21, storage22);

            Assert.AreNotEqual(storage11, storage21);
            Assert.AreNotEqual(storage11, storage22);
            Assert.AreNotEqual(storage12, storage21);
            Assert.AreNotEqual(storage12, storage22);
        }
    }
}
