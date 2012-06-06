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
        
        public DMServer(int port)
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            IPEndPoint localEP = new IPEndPoint(ipHostInfo.AddressList[0], port);
            Console.WriteLine("New socket listening at {0}:{1}", localEP.Address, localEP.Port);
            for (int i = 1; i < ipHostInfo.AddressList.Length; i++)
                Console.WriteLine("                        {0}:{1}", ipHostInfo.AddressList[i].ToString(), localEP.Port);
            _server = new Socket(localEP.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _server.Bind(localEP);
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

        private static int nextId = 0; //
        private void AcceptCallback(IAsyncResult result)
        {
            Console.WriteLine("A TM has found me!");
            Socket socket = (Socket)result.AsyncState;

            Socket receivingSocket = socket.EndAccept(result);

            int myId = nextId++; //

            while (receivingSocket.Connected)
            {
                BaseMessage message = SocketCommunicator.Receive(receivingSocket);

                Console.WriteLine("DM-"+myId+" just received a(n) "+message.GetType().Name);
                CommandMessage commandMessage = (CommandMessage) message;
                commandMessage.HandleCommandMessage(this, receivingSocket);
            }

        }

        private DataManager GetDM()
        {
            return DataManager.GetInstance();
        }

        #region ICommandMessageHandler
        public void HandleBeginMessage(BeginMessage beginMessage)
        {
            GetDM().Begin();
        }

        public void HandleEndMessage(EndMessage endMessage)
        {
            GetDM().End();
        }

        public void HandleReadMessage(ReadMessage readMessage, Socket socket)
        {
            byte readValue = GetDM().Read(readMessage.ObjectLocation);
            SocketCommunicator.Send(socket, new ReadResponse{Value = readValue});
        }

        public void HandleEndStagingPhaseMessage(EndStagingPhaseMessage endStagingPhaseMessage, Socket socket)
        {
            Vote vote = GetDM().EndStagingPhase();
            SocketCommunicator.Send(socket, new EndStagingPhaseResponse{Vote = vote});
        }

        public void HandleStageChangeMessage(StageChangeMessage stageChangeMessage)
        {
            GetDM().StageChange(stageChangeMessage.Change);
        }

        public void HandleAbortMessage(AbortMessage abortMessage)
        {
            GetDM().Abort();
        }

        public void HandleRestartMessage(RestartMessage restartMessage)
        {
            GetDM().Restart();
        }

        #endregion

    }
}
