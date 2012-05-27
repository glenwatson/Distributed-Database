using System;
using System.Net.Sockets;

namespace DistributedConcurrency.Shared.Communication.Messages.DMResponses
{
    [Serializable]
    public class EndStagingPhaseResponse : ResponseMessage
    {
        public Vote Vote { get; set; }
    }
}
