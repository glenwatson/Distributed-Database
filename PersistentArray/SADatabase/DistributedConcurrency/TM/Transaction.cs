using System.Collections.Generic;
using System.Linq;
using DistributedConcurrency.Shared;

namespace DistributedConcurrency.TM
{
    public class Transaction : IDatabase
    {
        private PrivateWorkspace _workspace;
        public byte Read(DataLocation dataLocation)
        {
            Change change = _workspace.GetDataLocation(dataLocation);
            byte value;
            if (change == null) // the DataLocation is not in the workspace
            {
                TransactionManager tm = TransactionManager.GetInstance();
                value = tm.Read(dataLocation);
                _workspace.AddChange(new Change(dataLocation, value, false));
            }
            else // the DataLocation is in the workspace
            {
                value = change.Value;
            }
            return value;
        }

        public void Write(DataLocation location, byte value)
        {
            Change change = _workspace.GetDataLocation(location);
            if (change == null) // the DataLocation is not in the workspace
            {
                _workspace.AddChange(new Change(location, value, true));
            }
            else // the DataLocation is in the workspace
            {
                change.Value = value;
            }
        }

        public void Begin()
        {
            _workspace = new PrivateWorkspace();
        }

        public void End()
        {
            TransactionManager tm = TransactionManager.GetInstance();
            HashSet<DMLocation> involvedDms = new HashSet<DMLocation>();
            foreach (Change change in _workspace)
            {
                tm.StageChange(change);
                involvedDms.Add(change.Location.DmLocation);
            }

            bool anyAbort = false;
            foreach (DMLocation involvedDm in involvedDms)
            {
                if (tm.EndStagingPhase(involvedDm) == Vote.Abort)
                {
                    anyAbort = true;
                    break;
                }
            }

            foreach (DMLocation involvedDm in involvedDms)
            {
                if(anyAbort)
                    tm.Abort(involvedDm);
                else
                    tm.End(involvedDm);
            }
        }

        public void Abort()
        {
            _workspace = null;
        }

        public void Restart()
        {
            _workspace.RemoveAll();
        }
    }
}
