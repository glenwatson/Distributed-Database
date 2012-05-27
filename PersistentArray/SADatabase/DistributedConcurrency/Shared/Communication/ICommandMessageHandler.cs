using System.Net.Sockets;
using DistributedConcurrency.Shared.Communication.Messages.TMMessages;

namespace DistributedConcurrency.Shared.Communication
{
    public interface ICommandMessageHandler
    {
        void HandleBeginMessage(BeginMessage beginMessage);
        void HandleEndMessage(EndMessage endMessage);
        void HandleReadMessage(ReadMessage readMessage, Socket socket);
        void HandleEndStagingPhaseMessage(EndStagingPhaseMessage endStagingPhaseMessage, Socket socket);
        void HandleStageChangeMessage(StageChangeMessage stageChangeMessage);
        void HandleAbortMessage(AbortMessage abortMessage);
        void HandleRestartMessage(RestartMessage restartMessage);
    }
}
