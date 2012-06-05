using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistributedConcurrency.DM.Journaling
{
    interface IJournalStorage<T> : IQueue<T>
    {
        void Recover();
        void RemoveAll();
    }
}
