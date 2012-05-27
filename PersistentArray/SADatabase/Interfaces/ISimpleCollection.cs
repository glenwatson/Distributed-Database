namespace Interfaces
{
    public interface ISimpleCollection
    {
        void WipeElement(int index);
        int GetElementSize();
        byte[] Get(int index);
        void Put(int index, byte[] buffer);
    }
}
