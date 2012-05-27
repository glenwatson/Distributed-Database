using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DistributedConcurrency;

namespace TransactionManager
{
    internal interface ITransactionManager
    {
        byte Read(DataLocation dataLocation);
        void Write(DataLocation dataLocation, byte value);
        void Begin(DMLocation dmLocation);
        void End(DMLocation dmLocation);
        void Abort(DMLocation dmLocation);
        void Restart(DMLocation dmLocation);
    }
}
