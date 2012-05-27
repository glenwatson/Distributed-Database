using System;

namespace DistributedConcurrency.Shared
{
    [Serializable]
    public enum Vote
    {
        Abort,
        Commit
    }
}
