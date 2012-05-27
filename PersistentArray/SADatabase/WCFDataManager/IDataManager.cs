using System.ServiceModel;
using DistributedConcurrency;
using Interfaces;

namespace WCFDataManager
{
    [ServiceContract]
    public interface IDataManager
    {
        [OperationContract]
        byte Read(DataLocation location);

        [OperationContract]
        void Write(DataLocation location, byte value);

        [OperationContract]
        void Begin();

        [OperationContract]
        void End();

        [OperationContract]
        void Abort();

        [OperationContract]
        void Restart();

        [OperationContract]
        byte StageChange(Change change);

        [OperationContract]
        Vote EndStagingPhase();
    }
}
