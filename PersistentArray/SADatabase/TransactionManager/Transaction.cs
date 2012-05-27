using System.Linq;
using DistributedConcurrency;
using Change = TMProxy.Change;
using DataLocation = TMProxy.DataLocation;

namespace TransactionManager
{
    public class Transaction : IDatabase
    {
        private PrivateWorkspace _workspace;
        public byte Read(DataLocation location)
        {
            Change change = _workspace.FirstOrDefault(c => c.Location == location);
            byte value;
            if (change == null) // the DataLocation is not in the workspace
            {
                _workspace.AddChange(new Change{IsWrite = false, Location = location});
            }
            else // the DataLocatoin is in the workspace
            {
                value = change.Value;
            }
        }

        public void Write(DataLocation location, byte value)
        {
            throw new System.NotImplementedException();
        }

        public void Begin()
        {
            _workspace = new PrivateWorkspace();
        }

        public void End()
        {
            throw new System.NotImplementedException();
        }

        public void Abort()
        {
            throw new System.NotImplementedException();
        }

        public void Restart()
        {
            throw new System.NotImplementedException();
        }
    }
}
