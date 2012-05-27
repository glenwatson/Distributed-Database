namespace Interfaces
{
    public interface IUserHeader
    {
        byte[] GetUserHeader();

        int GetUserHeaderSize();

        void PutUserHeader(byte[] userHeader);
    }
}
