using System;
using System.Net.Sockets;

namespace DistributedConcurrency.Shared.Communication.Messages.TMMessages
{
    [Serializable]
    public class EndStagingPhaseMessage : CommandMessage
    {
        public override void HandleCommandMessage(ICommandMessageHandler handler, Socket socket)
        {
            handler.HandleEndStagingPhaseMessage(this, socket);
        }
    }
}
