namespace Interfaces
{
    public interface IStorage
    {
        void Close();

        void WriteInt(int i);
        void WriteByte(byte b);
        void WriteByteArray(byte[] buffer);

        int ReadInt();
        byte ReadByte();
        void ReadByteArray(byte[] buffer);

        void Seek(int position);
        string GetFileName();
        void Flush();
    }
}
