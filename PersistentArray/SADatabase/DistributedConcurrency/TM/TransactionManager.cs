using DistributedConcurrency.Shared;

namespace DistributedConcurrency.TM
{
    class TransactionManager : ITransactionManager
    {
        private readonly DMCommunicator _communicator = new DMCommunicator();

        #region Singleton
        private static TransactionManager _tm;

        private TransactionManager() {}
        public static TransactionManager GetInstance()
        {
            return _tm ?? (_tm = new TransactionManager());
        }
        #endregion
        
        public byte Read(DataLocation dataLocation)
        {
            return _communicator.Read(dataLocation);
        }

        public void Begin(DMLocation dmLocation)
        {
            _communicator.Begin(dmLocation);
        }

        public void End(DMLocation dmLocation)
        {
            _communicator.End(dmLocation);
        }

        public void Abort(DMLocation dmLocation)
        {
            _communicator.Abort(dmLocation);
        }

        public void Restart(DMLocation dmLocation)
        {
            _communicator.Restart(dmLocation);
        }

        public void StageChange(Change change)
        {
            _communicator.StageChange(change);
        }

        public Vote EndStagingPhase(DMLocation dmLocation)
        {
            return _communicator.EndStagingPhase(dmLocation);
        }
    }
}
