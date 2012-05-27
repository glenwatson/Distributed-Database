using System;
using System.Net.Sockets;

namespace DistributedConcurrency.Shared.Communication.Messages.DMResponses
{
    [Serializable]
    public class ReadResponse : ResponseMessage
    {
        public byte Value { get; set; }
    }
}
