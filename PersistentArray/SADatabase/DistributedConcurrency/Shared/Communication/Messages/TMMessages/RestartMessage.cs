using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;

namespace DistributedConcurrency.Shared.Communication.Messages.TMMessages
{
    [Serializable]
    public class RestartMessage : CommandMessage
    {
        public override void HandleCommandMessage(ICommandMessageHandler handler, Socket socket)
        {
            handler.HandleRestartMessage(this);
        }
    }
}
