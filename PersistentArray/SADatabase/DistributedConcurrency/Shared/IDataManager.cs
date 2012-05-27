namespace DistributedConcurrency.Shared
{
    public interface IDataManager
    {
        byte Read(ObjectLocation objLocation);

        void Begin();

        void End();

        void Abort();

        void Restart();

        void StageChange(Change change);

        Vote EndStagingPhase();
    }
}
