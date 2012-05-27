// Type: System.NotImplementedException
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security;

namespace System
{
    [ComVisible(true)]
    [Serializable]
    public class NotImplementedException : SystemException
    {
        public NotImplementedException();
        public NotImplementedException(string message);
        public NotImplementedException(string message, Exception inner);

        [SecuritySafeCritical]
        protected NotImplementedException(SerializationInfo info, StreamingContext context);
    }
}
