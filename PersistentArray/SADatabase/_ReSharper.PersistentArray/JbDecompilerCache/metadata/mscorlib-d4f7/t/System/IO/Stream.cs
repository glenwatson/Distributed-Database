// Type: System.IO.Stream
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.IO
{
    [ComVisible(true)]
    [Serializable]
    public abstract class Stream : MarshalByRefObject, IDisposable
    {
        public static readonly Stream Null;
        protected Stream();
        public abstract bool CanRead { get; }
        public abstract bool CanSeek { get; }

        [ComVisible(false)]
        public virtual bool CanTimeout { get; }

        public abstract bool CanWrite { get; }
        public abstract long Length { get; }
        public abstract long Position { get; set; }

        [ComVisible(false)]
        public virtual int ReadTimeout { get; set; }

        [ComVisible(false)]
        public virtual int WriteTimeout { get; set; }

        #region IDisposable Members

        public void Dispose();

        #endregion

        public void CopyTo(Stream destination);
        public void CopyTo(Stream destination, int bufferSize);
        public virtual void Close();
        protected virtual void Dispose(bool disposing);
        public abstract void Flush();

        [Obsolete("CreateWaitHandle will be removed eventually.  Please use \"new ManualResetEvent(false)\" instead.")]
        protected virtual WaitHandle CreateWaitHandle();

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public virtual IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state);

        public virtual int EndRead(IAsyncResult asyncResult);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public virtual IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state);

        public virtual void EndWrite(IAsyncResult asyncResult);
        public abstract long Seek(long offset, SeekOrigin origin);
        public abstract void SetLength(long value);
        public abstract int Read([In, Out] byte[] buffer, int offset, int count);

        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public virtual int ReadByte();

        public abstract void Write(byte[] buffer, int offset, int count);
        public virtual void WriteByte(byte value);

        [HostProtection(SecurityAction.LinkDemand, Synchronization = true)]
        public static Stream Synchronized(Stream stream);

        protected virtual void ObjectInvariant();
    }
}
