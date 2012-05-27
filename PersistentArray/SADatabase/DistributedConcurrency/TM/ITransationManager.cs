using DistributedConcurrency.Shared;

namespace DistributedConcurrency.TM
{
    internal interface ITransactionManager
    {
        byte Read(DataLocation dataLocation);
        void Begin(DMLocation dmLocation);
        void End(DMLocation dmLocation);
        void Abort(DMLocation dmLocation);
        void Restart(DMLocation dmLocation);
        void StageChange(Change change);
        Vote EndStagingPhase(DMLocation dmLocation);
    }
}
