using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ByteHelperUnitTest
{
    public class TestHelper
    {
        public static void AssertByteArraysAreSame(byte[] expected, byte[] actual)
        {
            byte[] longer = expected;
            byte[] shorter = actual;
            if (actual.Length > expected.Length)
            {
                longer = actual;
                shorter = expected;
            }

            int i = 0;
            while (i < shorter.Length)
            {
                Assert.AreEqual(shorter[i], longer[i]);
                i++;
            }
            for (int j = i; j < longer.Length; j++)
            {
                Assert.AreEqual(0, longer[j]);
            }
        }
    }
}
