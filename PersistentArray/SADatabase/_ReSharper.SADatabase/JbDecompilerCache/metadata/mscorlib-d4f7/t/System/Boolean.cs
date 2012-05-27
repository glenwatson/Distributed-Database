// Type: System.Boolean
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System.Runtime.InteropServices;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public struct Boolean : IComparable, IConvertible, IComparable<bool>, IEquatable<bool>
    {
        public static readonly string TrueString;
        public static readonly string FalseString;

        #region IComparable Members

        public int CompareTo(object obj);

        #endregion

        #region IComparable<bool> Members

        public int CompareTo(bool value);

        #endregion

        #region IConvertible Members

        public string ToString(IFormatProvider provider);
        public TypeCode GetTypeCode();
        bool IConvertible.ToBoolean(IFormatProvider provider);
        char IConvertible.ToChar(IFormatProvider provider);
        sbyte IConvertible.ToSByte(IFormatProvider provider);
        byte IConvertible.ToByte(IFormatProvider provider);
        short IConvertible.ToInt16(IFormatProvider provider);
        ushort IConvertible.ToUInt16(IFormatProvider provider);
        int IConvertible.ToInt32(IFormatProvider provider);
        uint IConvertible.ToUInt32(IFormatProvider provider);
        long IConvertible.ToInt64(IFormatProvider provider);
        ulong IConvertible.ToUInt64(IFormatProvider provider);
        float IConvertible.ToSingle(IFormatProvider provider);
        double IConvertible.ToDouble(IFormatProvider provider);
        decimal IConvertible.ToDecimal(IFormatProvider provider);
        DateTime IConvertible.ToDateTime(IFormatProvider provider);
        object IConvertible.ToType(Type type, IFormatProvider provider);

        #endregion

        #region IEquatable<bool> Members

        public bool Equals(bool obj);

        #endregion

        public override int GetHashCode();
        public override string ToString();
        public override bool Equals(object obj);
        public static bool Parse(string value);
        public static bool TryParse(string value, out bool result);
    }
}
