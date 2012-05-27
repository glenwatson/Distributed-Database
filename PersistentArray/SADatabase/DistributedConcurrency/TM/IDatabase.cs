using DistributedConcurrency.Shared;

namespace DistributedConcurrency.TM
{
    public interface IDatabase
    {
        byte Read(DataLocation dataLocation);

        void Write(DataLocation dataLocation, byte value);

        void Begin();

        void End();

        void Abort();

        void Restart();
    }
}
