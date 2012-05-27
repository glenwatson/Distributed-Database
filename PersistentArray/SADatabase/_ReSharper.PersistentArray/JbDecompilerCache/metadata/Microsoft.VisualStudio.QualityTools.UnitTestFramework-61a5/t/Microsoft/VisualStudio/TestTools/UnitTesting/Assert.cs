// Type: Microsoft.VisualStudio.TestTools.UnitTesting.Assert
// Assembly: Microsoft.VisualStudio.QualityTools.UnitTestFramework, Version=10.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// Assembly location: c:\Program Files (x86)\Microsoft Visual Studio 10.0\Common7\IDE\PublicAssemblies\Microsoft.VisualStudio.QualityTools.UnitTestFramework.dll

using System;
using System.Globalization;

namespace Microsoft.VisualStudio.TestTools.UnitTesting
{
    public static class Assert
    {
        public static void IsTrue(bool condition);
        public static void IsTrue(bool condition, string message);
        public static void IsTrue(bool condition, string message, params object[] parameters);
        public static void IsFalse(bool condition);
        public static void IsFalse(bool condition, string message);
        public static void IsFalse(bool condition, string message, params object[] parameters);
        public static void IsNull(object value);
        public static void IsNull(object value, string message);
        public static void IsNull(object value, string message, params object[] parameters);
        public static void IsNotNull(object value);
        public static void IsNotNull(object value, string message);
        public static void IsNotNull(object value, string message, params object[] parameters);
        public static void AreSame(object expected, object actual);
        public static void AreSame(object expected, object actual, string message);
        public static void AreSame(object expected, object actual, string message, params object[] parameters);
        public static void AreNotSame(object notExpected, object actual);
        public static void AreNotSame(object notExpected, object actual, string message);
        public static void AreNotSame(object notExpected, object actual, string message, params object[] parameters);
        public static void AreEqual<T>(T expected, T actual);
        public static void AreEqual<T>(T expected, T actual, string message);
        public static void AreEqual<T>(T expected, T actual, string message, params object[] parameters);
        public static void AreNotEqual<T>(T notExpected, T actual);
        public static void AreNotEqual<T>(T notExpected, T actual, string message);
        public static void AreNotEqual<T>(T notExpected, T actual, string message, params object[] parameters);
        public static void AreEqual(object expected, object actual);
        public static void AreEqual(object expected, object actual, string message);
        public static void AreEqual(object expected, object actual, string message, params object[] parameters);
        public static void AreNotEqual(object notExpected, object actual);
        public static void AreNotEqual(object notExpected, object actual, string message);
        public static void AreNotEqual(object notExpected, object actual, string message, params object[] parameters);
        public static void AreEqual(float expected, float actual, float delta);
        public static void AreEqual(float expected, float actual, float delta, string message);
        public static void AreEqual(float expected, float actual, float delta, string message, params object[] parameters);
        public static void AreNotEqual(float notExpected, float actual, float delta);
        public static void AreNotEqual(float notExpected, float actual, float delta, string message);
        public static void AreNotEqual(float notExpected, float actual, float delta, string message, params object[] parameters);
        public static void AreEqual(double expected, double actual, double delta);
        public static void AreEqual(double expected, double actual, double delta, string message);
        public static void AreEqual(double expected, double actual, double delta, string message, params object[] parameters);
        public static void AreNotEqual(double notExpected, double actual, double delta);
        public static void AreNotEqual(double notExpected, double actual, double delta, string message);
        public static void AreNotEqual(double notExpected, double actual, double delta, string message, params object[] parameters);
        public static void AreEqual(string expected, string actual, bool ignoreCase);
        public static void AreEqual(string expected, string actual, bool ignoreCase, string message);
        public static void AreEqual(string expected, string actual, bool ignoreCase, string message, params object[] parameters);
        public static void AreEqual(string expected, string actual, bool ignoreCase, CultureInfo culture);
        public static void AreEqual(string expected, string actual, bool ignoreCase, CultureInfo culture, string message);
        public static void AreEqual(string expected, string actual, bool ignoreCase, CultureInfo culture, string message, params object[] parameters);
        public static void AreNotEqual(string notExpected, string actual, bool ignoreCase);
        public static void AreNotEqual(string notExpected, string actual, bool ignoreCase, string message);
        public static void AreNotEqual(string notExpected, string actual, bool ignoreCase, string message, params object[] parameters);
        public static void AreNotEqual(string notExpected, string actual, bool ignoreCase, CultureInfo culture);
        public static void AreNotEqual(string notExpected, string actual, bool ignoreCase, CultureInfo culture, string message);
        public static void AreNotEqual(string notExpected, string actual, bool ignoreCase, CultureInfo culture, string message, params object[] parameters);
        public static void IsInstanceOfType(object value, Type expectedType);
        public static void IsInstanceOfType(object value, Type expectedType, string message);
        public static void IsInstanceOfType(object value, Type expectedType, string message, params object[] parameters);
        public static void IsNotInstanceOfType(object value, Type wrongType);
        public static void IsNotInstanceOfType(object value, Type wrongType, string message);
        public static void IsNotInstanceOfType(object value, Type wrongType, string message, params object[] parameters);
        public static void Fail();
        public static void Fail(string message);
        public static void Fail(string message, params object[] parameters);
        public static void Inconclusive();
        public static void Inconclusive(string message);
        public static void Inconclusive(string message, params object[] parameters);
        public new static bool Equals(object objA, object objB);
        public static string ReplaceNullChars(string input);
    }
}
