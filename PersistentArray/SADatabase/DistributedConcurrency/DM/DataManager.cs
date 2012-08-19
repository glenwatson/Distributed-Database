using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using DistributedConcurrency.DM.Journaling;
using DistributedConcurrency.Shared;

namespace DistributedConcurrency.DM
{
    public class DataManager : IDataManager
    {
        private readonly PrivateWorkspace _workspace = new PrivateWorkspace();
        private readonly LockManager _lockManager = new LockManager();
        //TODO: get rid of hard coded path
        private readonly Journal<Change> _journal = new Journal<Change>(new Uri(@"C:\DB\Journal\"));
        private readonly ObjectReadWriter _readWriter = new ObjectReadWriter();
        private readonly DMServer _server;

        public DataManager(int port)
        {
            //TODO: Move to config file, or something....
            _server = new DMServer(port, this);
        }

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

        private static int changeCount = 0;
        public void End()
        {
            foreach (Change change in _workspace)
            {
                Console.WriteLine("+ on thread: " + Thread.CurrentThread.ManagedThreadId);
                changeCount++;
                _journal.AddChange(change); //journal the change
            }
            foreach (Change change in _workspace)
            {
                if(change.IsWrite)
                    Write(change.Location.ObjectLocation, change.Value);
                Console.WriteLine("- on thread: " + Thread.CurrentThread.ManagedThreadId);
                changeCount--;
                _journal.RemoveChange();
                _lockManager.RelaseLock(change.Location);
            }
            Console.WriteLine("Releasing _workspace");
            Monitor.Exit(_workspace);
            Console.WriteLine("Final files at of End(): "+changeCount);
        }

        public void Abort()
        {
            RemoveAllChanges();
            Console.WriteLine("Releasing _workspace");
            Monitor.Exit(_workspace);
        }

        public void Restart()
        {
            RemoveAllChanges();
        }

        private void RemoveAllChanges()
        {
            _workspace.RemoveAll();
            _journal.RemoveAll();
        }

        public void StageChange(Change change)
        {
            AssertDMLocationIsMe(change.Location.DmLocation);
            _workspace.AddChange(change);
        }

        //TODO: get rid of break
        public Vote EndStagingPhase()
        {
            Console.WriteLine("Locking _workspace");
            Monitor.Enter(_workspace);

            bool successfulStage = true;
            foreach (Change change in _workspace)
            {
                successfulStage = PrepStagedChange(successfulStage, change);
                if (!successfulStage)
                    break;
            }

            return successfulStage ? Vote.Commit : Vote.Abort;
        }

        private bool PrepStagedChange(bool successfulStage, Change change)
        {
            // if I can't get the lock, fail OR if change is a read & change's value is not what was there previously, fail
            if (!_lockManager.GetLock(change.Location) || (change.IsRead && (change.Value != Read(change.Location.ObjectLocation))))
            {
                successfulStage = false;
            }
            return successfulStage;
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
            //TODO: assert yourself!
        }

        public DMLocation GetLocation()
        {
            return new DMLocation(_server.Location.ToString());
        }
    }
}
