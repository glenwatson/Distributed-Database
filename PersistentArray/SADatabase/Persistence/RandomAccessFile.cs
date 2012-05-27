using System.IO;
using ByteHelper;
using Interfaces;

namespace Persistence
{
    public class RandomAccessFile : IStorage
    {
        private readonly FileStream _fileStream;

        public RandomAccessFile(FileStream stream)
        {
            _fileStream = stream;
        }

        //~RandomAccessFile()
        //{
        //    Close();
        //}

        public void Close()
        {
            if(_fileStream.CanWrite)
                _fileStream.Flush();
            _fileStream.Close();
        }

        public void WriteInt(int i)
        {
            byte[] array = i.ToBytes();
            _fileStream.Write(array, 0, array.Length);
            _fileStream.Flush();
        }

        public void WriteByte(byte b)
        {
            _fileStream.WriteByte(b);
            _fileStream.Flush();
        }

        public void WriteByteArray(byte[] buffer)
        {
            _fileStream.Write(buffer, 0, buffer.Length);
            _fileStream.Flush();
        }

        public int ReadInt()
        {
            byte[] bytes = new byte[4];
            _fileStream.Read(bytes, 0, bytes.Length);
            return bytes.ToInt();
        }

        public byte ReadByte()
        {
            return (byte) _fileStream.ReadByte();
        }

        public void ReadByteArray(byte[] buffer)
        {
            _fileStream.Read(buffer, 0, buffer.Length);
        }

        public void Seek(int position)
        {
            _fileStream.Seek(position, SeekOrigin.Begin);
        }

        public string GetFileName()
        {
            return _fileStream.Name;
        }

        public void Flush()
        {
            _fileStream.Flush();
        }
    }
}
