using System;
using System.Net.Sockets;

namespace DistributedConcurrency.Shared.Communication.Messages.TMMessages
{
    [Serializable]
    public class ReadMessage : CommandMessage
    {
        public ObjectLocation ObjectLocation { get; set; }
        public override void HandleCommandMessage(ICommandMessageHandler handler, Socket socket)
        {
            handler.HandleReadMessage(this, socket);
        }
    }
}
