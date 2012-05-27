// Type: System.Int32
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System.Globalization;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public struct Int32 : IComparable, IFormattable, IConvertible, IComparable<int>, IEquatable<int>
    {
        public const int MaxValue = 2147483647;
        public const int MinValue = -2147483648;

        #region IComparable Members

        public int CompareTo(object value);

        #endregion

        #region IComparable<int> Members

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public int CompareTo(int value);

        #endregion

        #region IConvertible Members

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        [SecuritySafeCritical]
        public string ToString(IFormatProvider provider);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public TypeCode GetTypeCode();

        bool IConvertible.ToBoolean(IFormatProvider provider);
        char IConvertible.ToChar(IFormatProvider provider);
        sbyte IConvertible.ToSByte(IFormatProvider provider);
        byte IConvertible.ToByte(IFormatProvider provider);
        short IConvertible.ToInt16(IFormatProvider provider);
        ushort IConvertible.ToUInt16(IFormatProvider provider);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        int IConvertible.ToInt32(IFormatProvider provider);

        uint IConvertible.ToUInt32(IFormatProvider provider);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        long IConvertible.ToInt64(IFormatProvider provider);

        ulong IConvertible.ToUInt64(IFormatProvider provider);
        float IConvertible.ToSingle(IFormatProvider provider);
        double IConvertible.ToDouble(IFormatProvider provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider);
        DateTime IConvertible.ToDateTime(IFormatProvider provider);
        object IConvertible.ToType(Type type, IFormatProvider provider);

        #endregion

        #region IEquatable<int> Members

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public bool Equals(int obj);

        #endregion

        #region IFormattable Members

        [SecuritySafeCritical]
        public string ToString(string format, IFormatProvider provider);

        #endregion

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public override bool Equals(object obj);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public override int GetHashCode();

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        [SecuritySafeCritical]
        public override string ToString();

        [SecuritySafeCritical]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public string ToString(string format);

        public static int Parse(string s);
        public static int Parse(string s, NumberStyles style);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static int Parse(string s, IFormatProvider provider);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static int Parse(string s, NumberStyles style, IFormatProvider provider);

        public static bool TryParse(string s, out int result);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool TryParse(string s, NumberStyles style, IFormatProvider provider, out int result);
    }
}
