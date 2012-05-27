using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using ByteHelper;
using DistributedConcurrency.Shared.Communication.Messages;

namespace DistributedConcurrency.Shared
{
    class SocketCommunicator
    {
        public static void Send(Socket socket, BaseMessage msg)
        {
            byte[] serialized = MessageSerialization.Serialize(msg);
            socket.Send(serialized.Length.ToBytes());
            socket.Send(serialized);
        }

        public static BaseMessage Receive(Socket socket)
        {
            byte[] byteSize = new byte[4];
            socket.Receive(byteSize);
            int size = MessageSerialization.GetMessageSize(byteSize);

            byte[] payload = new byte[size];
            socket.Receive(payload);
            BaseMessage message = MessageSerialization.Deserialze(payload);
            return message;
        }
    }
}
