using System;
using System.Net.Sockets;

namespace DistributedConcurrency.Shared.Communication.Messages.TMMessages
{
    [Serializable]
    public class BeginMessage : CommandMessage
    {
        public override void HandleCommandMessage(ICommandMessageHandler handler, Socket socket)
        {
            handler.HandleBeginMessage(this);
        }
    }
}
