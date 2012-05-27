using TMProxy;

namespace TransactionManager
{
    public interface IDatabase
    {
        byte Read(DataLocation location);

        void Write(DataLocation location, byte value);

        void Begin();

        void End();

        void Abort();

        void Restart();
    }
}
