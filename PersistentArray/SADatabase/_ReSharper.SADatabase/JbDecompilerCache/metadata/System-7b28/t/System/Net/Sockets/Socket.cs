// Type: System.Net.Sockets.Socket
// Assembly: System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089
// Assembly location: C:\Windows\Microsoft.NET\Framework\v4.0.30319\System.dll

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Security.Permissions;

namespace System.Net.Sockets
{
    public class Socket : IDisposable
    {
        public Socket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType);
        public Socket(SocketInformation socketInformation);

        [Obsolete("SupportsIPv4 is obsoleted for this type, please use OSSupportsIPv4 instead. http://go.microsoft.com/fwlink/?linkid=14202")]
        public static bool SupportsIPv4 { get; }

        public static bool OSSupportsIPv4 { get; }

        [Obsolete("SupportsIPv6 is obsoleted for this type, please use OSSupportsIPv6 instead. http://go.microsoft.com/fwlink/?linkid=14202")]
        public static bool SupportsIPv6 { get; }

        public static bool OSSupportsIPv6 { get; }
        public int Available { get; }
        public EndPoint LocalEndPoint { get; }
        public EndPoint RemoteEndPoint { get; }
        public IntPtr Handle { get; }
        public bool Blocking { get; set; }
        public bool UseOnlyOverlappedIO { get; set; }
        public bool Connected { get; }
        public AddressFamily AddressFamily { get; }
        public SocketType SocketType { get; }
        public ProtocolType ProtocolType { get; }
        public bool IsBound { get; }
        public bool ExclusiveAddressUse { get; set; }
        public int ReceiveBufferSize { get; set; }
        public int SendBufferSize { get; set; }
        public int ReceiveTimeout { get; set; }
        public int SendTimeout { get; set; }
        public LingerOption LingerState { get; set; }
        public bool NoDelay { get; set; }
        public short Ttl { get; set; }
        public bool DontFragment { get; set; }
        public bool MulticastLoopback { get; set; }
        public bool EnableBroadcast { get; set; }

        #region IDisposable Members

        public void Dispose();

        #endregion

        public void Bind(EndPoint localEP);
        public void Connect(EndPoint remoteEP);
        public void Connect(IPAddress address, int port);
        public void Connect(string host, int port);
        public void Connect(IPAddress[] addresses, int port);
        public void Close();
        public void Close(int timeout);
        public void Listen(int backlog);
        public Socket Accept();
        public int Send(byte[] buffer, int size, SocketFlags socketFlags);
        public int Send(byte[] buffer, SocketFlags socketFlags);
        public int Send(byte[] buffer);
        public int Send(IList<ArraySegment<byte>> buffers);
        public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags);
        public int Send(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode);
        public void SendFile(string fileName);
        public void SendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags);
        public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags);
        public int Send(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode);
        public int SendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP);
        public int SendTo(byte[] buffer, int size, SocketFlags socketFlags, EndPoint remoteEP);
        public int SendTo(byte[] buffer, SocketFlags socketFlags, EndPoint remoteEP);
        public int SendTo(byte[] buffer, EndPoint remoteEP);
        public int Receive(byte[] buffer, int size, SocketFlags socketFlags);
        public int Receive(byte[] buffer, SocketFlags socketFlags);
        public int Receive(byte[] buffer);
        public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags);
        public int Receive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode);
        public int Receive(IList<ArraySegment<byte>> buffers);
        public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags);
        public int Receive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode);
        public int ReceiveMessageFrom(byte[] buffer, int offset, int size, ref SocketFlags socketFlags, ref EndPoint remoteEP, out IPPacketInformation ipPacketInformation);
        public int ReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP);
        public int ReceiveFrom(byte[] buffer, int size, SocketFlags socketFlags, ref EndPoint remoteEP);
        public int ReceiveFrom(byte[] buffer, SocketFlags socketFlags, ref EndPoint remoteEP);
        public int ReceiveFrom(byte[] buffer, ref EndPoint remoteEP);
        public int IOControl(int ioControlCode, byte[] optionInValue, byte[] optionOutValue);
        public int IOControl(IOControlCode ioControlCode, byte[] optionInValue, byte[] optionOutValue);
        public void SetIPProtectionLevel(IPProtectionLevel level);
        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionValue);
        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue);
        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, bool optionValue);
        public void SetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, object optionValue);
        public object GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName);
        public void GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, byte[] optionValue);
        public byte[] GetSocketOption(SocketOptionLevel optionLevel, SocketOptionName optionName, int optionLength);
        public bool Poll(int microSeconds, SelectMode mode);
        public static void Select(IList checkRead, IList checkWrite, IList checkError, int microSeconds);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginSendFile(string fileName, AsyncCallback callback, object state);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginConnect(EndPoint remoteEP, AsyncCallback callback, object state);

        public SocketInformation DuplicateAndClose(int targetProcessId);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginConnect(string host, int port, AsyncCallback requestCallback, object state);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginConnect(IPAddress address, int port, AsyncCallback requestCallback, object state);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginConnect(IPAddress[] addresses, int port, AsyncCallback requestCallback, object state);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginDisconnect(bool reuseSocket, AsyncCallback callback, object state);

        public void Disconnect(bool reuseSocket);
        public void EndConnect(IAsyncResult asyncResult);
        public void EndDisconnect(IAsyncResult asyncResult);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginSend(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginSendFile(string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags, AsyncCallback callback, object state);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginSend(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state);

        public int EndSend(IAsyncResult asyncResult);
        public int EndSend(IAsyncResult asyncResult, out SocketError errorCode);
        public void EndSendFile(IAsyncResult asyncResult);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginSendTo(byte[] buffer, int offset, int size, SocketFlags socketFlags, EndPoint remoteEP, AsyncCallback callback, object state);

        public int EndSendTo(IAsyncResult asyncResult);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, AsyncCallback callback, object state);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginReceive(byte[] buffer, int offset, int size, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, AsyncCallback callback, object state);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginReceive(IList<ArraySegment<byte>> buffers, SocketFlags socketFlags, out SocketError errorCode, AsyncCallback callback, object state);

        public int EndReceive(IAsyncResult asyncResult);
        public int EndReceive(IAsyncResult asyncResult, out SocketError errorCode);
        public IAsyncResult BeginReceiveMessageFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback callback, object state);
        public int EndReceiveMessageFrom(IAsyncResult asyncResult, ref SocketFlags socketFlags, ref EndPoint endPoint, out IPPacketInformation ipPacketInformation);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginReceiveFrom(byte[] buffer, int offset, int size, SocketFlags socketFlags, ref EndPoint remoteEP, AsyncCallback callback, object state);

        public int EndReceiveFrom(IAsyncResult asyncResult, ref EndPoint endPoint);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginAccept(AsyncCallback callback, object state);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginAccept(int receiveSize, AsyncCallback callback, object state);

        [HostProtection(SecurityAction.LinkDemand, ExternalThreading = true)]
        public IAsyncResult BeginAccept(Socket acceptSocket, int receiveSize, AsyncCallback callback, object state);

        public Socket EndAccept(IAsyncResult asyncResult);
        public Socket EndAccept(out byte[] buffer, IAsyncResult asyncResult);
        public Socket EndAccept(out byte[] buffer, out int bytesTransferred, IAsyncResult asyncResult);
        public void Shutdown(SocketShutdown how);
        protected virtual void Dispose(bool disposing);
        ~Socket();
        public bool AcceptAsync(SocketAsyncEventArgs e);
        public bool ConnectAsync(SocketAsyncEventArgs e);
        public static bool ConnectAsync(SocketType socketType, ProtocolType protocolType, SocketAsyncEventArgs e);
        public static void CancelConnectAsync(SocketAsyncEventArgs e);
        public bool DisconnectAsync(SocketAsyncEventArgs e);
        public bool ReceiveAsync(SocketAsyncEventArgs e);
        public bool ReceiveFromAsync(SocketAsyncEventArgs e);
        public bool ReceiveMessageFromAsync(SocketAsyncEventArgs e);
        public bool SendAsync(SocketAsyncEventArgs e);
        public bool SendPacketsAsync(SocketAsyncEventArgs e);
        public bool SendToAsync(SocketAsyncEventArgs e);
    }
}
