using System;
using System.Net;
using System.Net.Sockets;
using DistributedConcurrency.Shared;
using DistributedConcurrency.Shared.Communication;
using DistributedConcurrency.Shared.Communication.Messages;
using DistributedConcurrency.Shared.Communication.Messages.DMResponses;
using DistributedConcurrency.Shared.Communication.Messages.TMMessages;

namespace DistributedConcurrency.DM
{
    public class DMServer : ICommandMessageHandler
    {
        private readonly Socket _server;
        private readonly IDataManager _dataManager;

        public EndPoint Location
        {
            get
            {
                return _server.LocalEndPoint;
            }
        }

        public DMServer(int port, IDataManager dataManager)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //TODO: get rid of localhost
            IPAddress ipAddr = new IPAddress(0x0100007f);
            IPEndPoint localEP = new IPEndPoint(ipAddr/*ipHostInfo.AddressList[0]*/, port);
            _server = new Socket(localEP.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _server.Bind(localEP);

            _dataManager = dataManager;
        }

        public void Start(int backlog)
        {
            _server.Listen(backlog);
            _server.BeginAccept(new AsyncCallback(AcceptCallback), _server);
        }

        public void Stop()
        {
            _server.Shutdown(SocketShutdown.Receive);
            _server.Close();
        }

        private void AcceptCallback(IAsyncResult result)
        {
            Socket socket = (Socket)result.AsyncState;

            Socket receivingSocket = socket.EndAccept(result);

            while (receivingSocket.Connected)
            {
                BaseMessage message = SocketCommunicator.Receive(receivingSocket);

                CommandMessage commandMessage = (CommandMessage) message;
                commandMessage.HandleCommandMessage(this, receivingSocket);
            }

        }

        #region ICommandMessageHandler
        public void HandleBeginMessage(BeginMessage beginMessage)
        {
            _dataManager.Begin();
        }

        public void HandleEndMessage(EndMessage endMessage)
        {
            _dataManager.End();
        }

        public void HandleReadMessage(ReadMessage readMessage, Socket socket)
        {
            byte readValue = _dataManager.Read(readMessage.ObjectLocation);
            SocketCommunicator.Send(socket, new ReadResponse{Value = readValue});
        }

        public void HandleEndStagingPhaseMessage(EndStagingPhaseMessage endStagingPhaseMessage, Socket socket)
        {
            Vote vote = _dataManager.EndStagingPhase();
            SocketCommunicator.Send(socket, new EndStagingPhaseResponse{Vote = vote});
        }

        public void HandleStageChangeMessage(StageChangeMessage stageChangeMessage)
        {
            _dataManager.StageChange(stageChangeMessage.Change);
        }

        public void HandleAbortMessage(AbortMessage abortMessage)
        {
            _dataManager.Abort();
        }

        public void HandleRestartMessage(RestartMessage restartMessage)
        {
            _dataManager.Restart();
        }

        #endregion

    }
}
