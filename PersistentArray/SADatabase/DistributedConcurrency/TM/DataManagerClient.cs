using System;
using System.Net.Sockets;
using DistributedConcurrency.DM;
using DistributedConcurrency.Shared;
using DistributedConcurrency.Shared.Communication.Messages;
using DistributedConcurrency.Shared.Communication.Messages.DMResponses;
using DistributedConcurrency.Shared.Communication.Messages.TMMessages;

namespace DistributedConcurrency.TM
{
    public class DataManagerClient : IDataManager
    {
        private readonly Socket _socket;

        public DataManagerClient(DMLocation dmLocation)
        {
            _socket = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(dmLocation.URI.Host, dmLocation.URI.Port);
        }

        public byte Read(ObjectLocation objLocation)
        {
            SocketCommunicator.Send(_socket, new ReadMessage { ObjectLocation = objLocation });
            return GetReadResponse().Value;
        }

        private ReadResponse GetReadResponse()
        {
            object message = SocketCommunicator.Receive(_socket);
            return (ReadResponse) message;
        }

        public void Begin()
        {
            SocketCommunicator.Send(_socket, new BeginMessage());
        }

        public void End()
        {
            SocketCommunicator.Send(_socket, new EndMessage());
        }

        public void Abort()
        {
            SocketCommunicator.Send(_socket, new AbortMessage());
        }

        public void Restart()
        {
            SocketCommunicator.Send(_socket, new RestartMessage());
        }

        public void StageChange(Change change)
        {
            SocketCommunicator.Send(_socket, new StageChangeMessage { Change = change });
        }

        public Vote EndStagingPhase()
        {
            SocketCommunicator.Send(_socket, new EndStagingPhaseMessage());
            return GetEndStagingPhaseResponse().Vote;
        }

        private EndStagingPhaseResponse GetEndStagingPhaseResponse()
        {
            object message = SocketCommunicator.Receive(_socket);
            return (EndStagingPhaseResponse) message;
        }
    }
}
