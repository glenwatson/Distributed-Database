using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Persistence;

namespace PersistenceUnitTest
{
    [TestClass]
    public class SlowRAFTest
    {
        [TestMethod]
        public void WriteIntTest()
        {
            SlowRAF raf = new SlowRAF(File.Open("WriteInt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None));
            try
            {
                AssertLongerThanASecond(() => raf.WriteInt(0));
                AssertLongerThanASecond(() => raf.WriteByte(0));
                AssertLongerThanASecond(() => raf.WriteByteArray(new byte[0]));
                AssertLongerThanASecond(() => raf.ReadInt());
                AssertLongerThanASecond(() => raf.ReadByte());
                AssertLongerThanASecond(() => raf.ReadByteArray(new byte[0]));
                AssertLongerThanASecond(() => raf.WriteInt(1));
            }
            finally
            {
                raf.Close();
            }
        }

        public void AssertLongerThanASecond(Action work)
        {
            DateTime startTime = DateTime.Now;
            work.Invoke();
            DateTime stopTime = DateTime.Now;
            TimeSpan delta = stopTime.Subtract(startTime);
            Assert.IsTrue(delta.Seconds >= 1, "should be greater than or equal to 1, is actually "+delta.Seconds);
        }
    }
}
