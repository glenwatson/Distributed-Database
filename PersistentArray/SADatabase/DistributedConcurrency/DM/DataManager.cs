using System;
using System.Linq;
using System.Threading;
using DistributedConcurrency.Shared;

namespace DistributedConcurrency.DM
{
    public class DataManager : IDataManager
    {
        private readonly PrivateWorkspace _workspace = new PrivateWorkspace();
        private readonly LockManager _lockManager = new LockManager();
        private readonly ObjectReadWriter _readWriter = new ObjectReadWriter();
        private readonly DMServer _server = new DMServer(11000);

        #region Singleton
        private static DataManager _dm;

        private DataManager(){}
        public static DataManager GetInstance()
        {
            return _dm ?? (_dm = new DataManager());
        }

        #endregion

        private void Write(ObjectLocation objLocation, byte value)
        {
            _readWriter.Write(objLocation, value);
        }

        #region IDataManager
        public byte Read(ObjectLocation objLocation)
        {
            return _readWriter.Read(objLocation);
        }

        public void Begin()
        {
            
        }

        public void End()
        {
            foreach (Change change in _workspace)
            {
                if(change.IsWrite)
                    Write(change.Location.ObjectLocation, change.Value);
                _lockManager.RelaseLock(change.Location);
            }
            Monitor.Exit(_workspace);
        }

        public void Abort()
        {
            Restart();
            Monitor.Exit(_workspace);
        }

        public void Restart()
        {
            _workspace.RemoveAll();
        }

        public void StageChange(Change change)
        {
            AssertDMLocationIsMe(change.Location.DmLocation);
            _workspace.AddChange(change);
        }

        //TODO: get rid of break
        public Vote EndStagingPhase()
        {
            Monitor.Enter(_workspace);

            bool successfulStage = true;
            foreach (Change change in _workspace)
            {
                if (!_lockManager.GetLock(change.Location) || (change.IsRead && (change.Value != Read(change.Location.ObjectLocation)))) // if I can't get the lock, fail OR if change is a read & change's value is not what was there previously, fail
                {
                    successfulStage = false;
                    break;
                }
            }

            return successfulStage ? Vote.Commit : Vote.Abort;
        }
        #endregion

        public void Start()
        {
            _server.Start(10);
        }

        public void Stop()
        {
            _server.Stop();
        }

        private void AssertDMLocationIsMe(DMLocation dmLocation)
        {

        }
    }
}
