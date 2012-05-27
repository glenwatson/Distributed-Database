using System;
using System.Net.Sockets;

namespace DistributedConcurrency.Shared.Communication.Messages.TMMessages
{
    [Serializable]
    public abstract class CommandMessage : BaseMessage
    {
        public abstract void HandleCommandMessage(ICommandMessageHandler handler, Socket socket);
    }
}
