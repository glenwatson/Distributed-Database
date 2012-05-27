using System;
using System.Net.Sockets;

namespace DistributedConcurrency.Shared.Communication.Messages.TMMessages
{
    [Serializable]
    public class StageChangeMessage : CommandMessage
    {
        public Change Change { get; set; }
        public override void HandleCommandMessage(ICommandMessageHandler handler, Socket socket)
        {
            handler.HandleStageChangeMessage(this);
        }
    }
}
