using System;
using DistributedConcurrency;

namespace TransactionManager
{
    class TransactionManager : ITransactionManager
    {
        private readonly DMCommunicator _communicator = new DMCommunicator();
        private TransactionManager() {}

        private static TransactionManager _tm;
        public static TransactionManager GetInstance()
        {
            if (_tm == null)
                _tm = new TransactionManager();
            return _tm;
        }
        public byte Read(DataLocation dataLocation)
        {
            return _communicator.Read(dataLocation);
        }

        public void Write(DataLocation dataLocation, byte value)
        {
            _communicator.Write(dataLocation, value);
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
    }
}
