using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using DistributedConcurrency.Shared;
using DistributedConcurrency.Shared.Communication;
using DistributedConcurrency.Shared.Communication.Messages;
using DistributedConcurrency.Shared.Communication.Messages.DMResponses;
using DistributedConcurrency.Shared.Communication.Messages.TMMessages;

namespace DistributedConcurrency.DM
{
    public class DMServer : ICommandMessageHandler
    {
        private readonly Socket _listeningSocket;
        private readonly IDataManager _dataManager;
        private bool acceptConnections;

        public EndPoint Location
        {
            get
            {
                return _listeningSocket.LocalEndPoint;
            }
        }

        public DMServer(int port, IDataManager dataManager)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            //TODO: get rid of localhost (0x0100007f)
            IPAddress ipAddr = new IPAddress(0x0100007f);
            IPEndPoint localEP = new IPEndPoint(ipAddr/*ipHostInfo.AddressList[0]*/, port);
            _listeningSocket = new Socket(localEP.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _listeningSocket.Bind(localEP);

            _dataManager = dataManager;
        }

        public void Start(int backlog)
        {
            _listeningSocket.Listen(backlog); //Start listening for connections and queuing them up
            acceptConnections = true; //Get ready to start accepting connections
            ThreadPool.QueueUserWorkItem(Run); //Spawn thread to accept connections
        }

        private void Run(object state)
        {
            while (acceptConnections)
            {
                try
                {
                    Socket receivingSocket = _listeningSocket.Accept();
                    ThreadPool.QueueUserWorkItem(HandleConnection, receivingSocket); //Spawn thread to handle the connection
                }
                catch (SocketException) //.Accept() will throw a SocketException when _listeningSocket is closed in the Stop()
                {}
            }
        }

        private void HandleConnection(object receivingSocketObj)
        {
            Socket receivingSocket = (Socket)receivingSocketObj;

            while (receivingSocket.Connected)
            {
                BaseMessage message = SocketCommunicator.Receive(receivingSocket);

                CommandMessage commandMessage = (CommandMessage) message;
                commandMessage.HandleCommandMessage(this, receivingSocket);
            }
            Console.WriteLine("Conn closed");
        }

        public void Stop()
        {
            acceptConnections = false;
            _listeningSocket.Close();
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
