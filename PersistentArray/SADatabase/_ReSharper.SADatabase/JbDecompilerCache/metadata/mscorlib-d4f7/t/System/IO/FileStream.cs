// Type: System.IO.FileStream
// Assembly: mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\mscorlib.dll

using Microsoft.Win32.SafeHandles;
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.AccessControl;
using System.Security.Permissions;

namespace System.IO
{
    [ComVisible(true)]
    public class FileStream : Stream
    {
        [SecuritySafeCritical]
        public FileStream(string path, FileMode mode);

        [SecuritySafeCritical]
        public FileStream(string path, FileMode mode, FileAccess access);

        [SecuritySafeCritical]
        public FileStream(string path, FileMode mode, FileAccess access, FileShare share);

        [SecuritySafeCritical]
        public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize);

        [SecuritySafeCritical]
        public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options);

        [SecuritySafeCritical]
        public FileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync);

        [SecuritySafeCritical]
        public FileStream(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options, FileSecurity fileSecurity);

        [SecuritySafeCritical]
        public FileStream(string path, FileMode mode, FileSystemRights rights, FileShare share, int bufferSize, FileOptions options);

        [SecuritySafeCritical]
        [Obsolete("This constructor has been deprecated.  Please use new FileStream(SafeFileHandle handle, FileAccess access) instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
        public FileStream(IntPtr handle, FileAccess access);

        [Obsolete("This constructor has been deprecated.  Please use new FileStream(SafeFileHandle handle, FileAccess access) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed.  http://go.microsoft.com/fwlink/?linkid=14202")]
        [SecuritySafeCritical]
        public FileStream(IntPtr handle, FileAccess access, bool ownsHandle);

        [Obsolete("This constructor has been deprecated.  Please use new FileStream(SafeFileHandle handle, FileAccess access, int bufferSize) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed.  http://go.microsoft.com/fwlink/?linkid=14202")]
        [SecuritySafeCritical]
        public FileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize);

        [SecuritySafeCritical]
        [Obsolete("This constructor has been deprecated.  Please use new FileStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync) instead, and optionally make a new SafeFileHandle with ownsHandle=false if needed.  http://go.microsoft.com/fwlink/?linkid=14202")]
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public FileStream(IntPtr handle, FileAccess access, bool ownsHandle, int bufferSize, bool isAsync);

        [SecuritySafeCritical]
        public FileStream(SafeFileHandle handle, FileAccess access);

        [SecuritySafeCritical]
        public FileStream(SafeFileHandle handle, FileAccess access, int bufferSize);

        [SecuritySafeCritical]
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public FileStream(SafeFileHandle handle, FileAccess access, int bufferSize, bool isAsync);

        public override bool CanRead { get; }
        public override bool CanWrite { get; }
        public override bool CanSeek { get; }
        public virtual bool IsAsync { get; }

        public override long Length { [SecuritySafeCritical]
        get; }

        public string Name { [SecuritySafeCritical]
        get; }

        public override long Position { [SecuritySafeCritical]
        get; [SecuritySafeCritical]
        set; }

        [Obsolete("This property has been deprecated.  Please use FileStream\'s SafeFileHandle property instead.  http://go.microsoft.com/fwlink/?linkid=14202")]
        public virtual IntPtr Handle { [SecurityCritical, SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        get; }

        public virtual SafeFileHandle SafeFileHandle { [SecurityCritical, SecurityPermission(SecurityAction.InheritanceDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        get; }

        [SecuritySafeCritical]
        public FileSecurity GetAccessControl();

        [SecuritySafeCritical]
        public void SetAccessControl(FileSecurity fileSecurity);

        [SecuritySafeCritical]
        protected override void Dispose(bool disposing);

        [SecuritySafeCritical]
        ~FileStream();

        [SecuritySafeCritical]
        public override void Flush();

        [SecuritySafeCritical]
        public virtual void Flush(bool flushToDisk);

        [SecuritySafeCritical]
        public override void SetLength(long value);

        [SecuritySafeCritical]
        public override int Read([In, Out] byte[] array, int offset, int count);

        [SecuritySafeCritical]
        public override long Seek(long offset, SeekOrigin origin);

        [SecuritySafeCritical]
        public override void Write(byte[] array, int offset, int count);

        [SecuritySafeCritical]
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public override IAsyncResult BeginRead(byte[] array, int offset, int numBytes, AsyncCallback userCallback, object stateObject);

        [SecuritySafeCritical]
        public override int EndRead(IAsyncResult asyncResult);

        [SecuritySafeCritical]
        public override int ReadByte();

        [SecuritySafeCritical]
        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public override IAsyncResult BeginWrite(byte[] array, int offset, int numBytes, AsyncCallback userCallback, object stateObject);

        [SecuritySafeCritical]
        public override void EndWrite(IAsyncResult asyncResult);

        [SecuritySafeCritical]
        public override void WriteByte(byte value);

        [SecuritySafeCritical]
        public virtual void Lock(long position, long length);

        [SecuritySafeCritical]
        public virtual void Unlock(long position, long length);
    }
}
