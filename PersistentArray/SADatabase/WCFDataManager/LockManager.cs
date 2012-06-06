using System.Collections.Generic;
using DistributedConcurrency;
using DistributedConcurrency.Shared;

namespace WCFDataManager
{
    public class LockManager
    {
        private readonly ISet<Lock> _locks = new HashSet<Lock>();

        public Lock GetLock(DataLocation location)
        {
            Lock newLock;
            lock (_locks)
            {
                foreach (Lock takenLock in _locks)
                {
                    if (takenLock.Overlap(location))
                        return null;
                }
                newLock = new Lock(location);
                _locks.Add(newLock);
            }
            return newLock;
        }

        public void RelaseLock(Lock ownedLock)
        {
            lock (_locks)
            {
                _locks.Remove(ownedLock);
            }
        }
    }
}
