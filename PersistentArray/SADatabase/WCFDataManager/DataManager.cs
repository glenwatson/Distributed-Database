using System;
using DistributedConcurrency;

namespace WCFDataManager
{
    public class DataManager : IDataManager
    {
        private PrivateWorkspace _workspace = new PrivateWorkspace();
        private LockManager _lockManager = new LockManager();
        public byte Read(DataLocation location)
        {
            throw new NotImplementedException();
        }

        public void Write(DataLocation location, byte value)
        {
            throw new NotImplementedException();
        }

        public void Begin()
        {
            throw new NotImplementedException();
        }

        public void End()
        {
            throw new NotImplementedException();
        }

        public void Abort()
        {
            throw new NotImplementedException();
        }

        public void Restart()
        {
            throw new NotImplementedException();
        }

        public byte StageChange(Change change)
        {
            throw new NotImplementedException();
        }

        public Vote EndStagingPhase()
        {
            throw new NotImplementedException();
        }
    }
}
