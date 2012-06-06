using System.Collections.Generic;
using System.Linq;
using DistributedConcurrency.Shared;

namespace DistributedConcurrency.DM
{
    public class LockManager
    {
        private readonly ISet<Lock> _locks = new HashSet<Lock>();

        public bool GetLock(DataLocation dataLocation)
        {
            System.Console.WriteLine("Locking " + dataLocation.GetHashCode());
            lock (_locks)
            {
                if (_locks.Any(takenLock => takenLock.Overlap(dataLocation)))
                {
                    return false;
                }
                Lock newLock = new Lock(dataLocation);
                _locks.Add(newLock);
            }
            return true;
        }

        public void RelaseLock(DataLocation dataLocation)
        {
            System.Console.WriteLine("Releasing " + dataLocation.GetHashCode());
            lock (_locks)
            {
                _locks.Remove(new Lock(dataLocation));
            }
        }
    }
}
