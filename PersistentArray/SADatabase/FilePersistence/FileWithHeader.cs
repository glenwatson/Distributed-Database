using System;
using System.IO;
using ByteHelper;
using FilePersistence.Exceptions;
using Interfaces;
using Persistence;

namespace FilePersistence
{
    public class FileWithHeader : IUserHeader, ICloseable, INextSpace
    {
        private const String StorageFolder = @"C:\DB\"; 
        private readonly IStorage _storage;

        public FileWithHeader(string fileName, int userHeaderSize)
        {
            String filePath = GetPathToName(fileName);
            if (File.Exists(filePath))
            {
                throw new FileNameConflictException(filePath + " already exists");
            }
            _storage = OpenStorage(filePath);

            if (userHeaderSize < 0)
                throw new InvalidUserHeaderException("The user header size must be positive");

            PutUserHeaderSize(userHeaderSize);
            PutUserHeader(new byte[userHeaderSize]);
            WriteNextIndex(GetHeaderSizeOffset());
        }

        public FileWithHeader(String fileName)
        {
            string filePath = GetPathToName(fileName);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("Could not find file " + filePath);
            }
            _storage = OpenStorage(filePath);
        }

        private static int GetNextIndexPosition()
        {
            return 0;
        }

        private static int GetUserHeaderPosition()
        {
            return GetNextIndexPosition() + PersistenceConstants.IntSize;
        }

        public static string GetStorageExt()
        {
            return "db";
        }

        public static String GetPathToName(String arrayName)
        {
            if(string.IsNullOrEmpty(arrayName))
                throw new FileNotFoundException();
            return StorageFolder + arrayName + '.' + GetStorageExt();
        }

        private IStorage OpenStorage(String filePath)
        {
            return StorageMultiton.GetInstance(filePath);
        }  

        public void Delete()
        {
            String filePath = _storage.GetFileName();
            StorageMultiton.RemoveInstance(filePath);
            _storage.Close();
            File.Delete(filePath);
        }

        public void Close()
        {
            _storage.Flush();
            //_storage.Close();
        }

        public int GetUserHeaderSize()
        {
            _storage.Seek(GetUserHeaderPosition());
            byte[] userHeaderSizeBytes = new byte[GetHeaderSize()];
            _storage.ReadByteArray(userHeaderSizeBytes);
            return userHeaderSizeBytes.ToInt();
        }

        private int GetHeaderSize()
        {
            return PersistenceConstants.IntSize;
        }

        public byte[] GetUserHeader()
        {
            byte[] userHeaderBytes = new byte[GetUserHeaderSize()];
            _storage.ReadByteArray(userHeaderBytes);
            return userHeaderBytes;
        }

        private void SetNextIndex(int index)
        {
            WriteNextIndex(index);
        }

        private int GetActualNextIndex()
        {
            _storage.Seek(GetNextIndexPosition());
            byte[] nextIndexBytes = new byte[PersistenceConstants.IntSize];
            _storage.ReadByteArray(nextIndexBytes);
            return nextIndexBytes.ToInt();
        }

        private void WriteNextIndex(int nextIndex)
        {
            _storage.Seek(GetNextIndexPosition());
            byte[] nextIndexBytes = nextIndex.ToBytes();
            _storage.WriteByteArray(nextIndexBytes);
        }

        public byte Get(int index)
        {
            int actualIndex = GetRealIndex(index);
            _storage.Seek(actualIndex);
            return _storage.ReadByte();
        }

        public byte[] GetAmount(int index, int amount)
        {
            int actualIndex = GetRealIndex(index);
            _storage.Seek(actualIndex);
            byte[] buffer = new byte[amount];
            _storage.ReadByteArray(buffer);
            return buffer;
        }

        public byte[] GetRange(int startIndex, int endIndex)
        {
            int actualStartIndex = GetRealIndex(startIndex);
            _storage.Seek(actualStartIndex);
            byte[] buffer = new byte[endIndex - startIndex + 1];
            _storage.ReadByteArray(buffer);
            return buffer;
        }

        public void Put(int index, byte data)
        {
            int actualIndex = GetRealIndex(index);
            _storage.Seek(actualIndex);
            _storage.WriteByte(data);
            if (actualIndex >= GetNextIndex())
                SetNextIndex(actualIndex + 1);
        }

        public void Put(int index, byte[] data)
        {
            int actualIndex = GetRealIndex(index);
            _storage.Seek(actualIndex);
            _storage.WriteByteArray(data);
            int lastIndexWrittenAt = actualIndex + data.Length-1;
            if (lastIndexWrittenAt >= GetActualNextIndex())
                SetNextIndex(lastIndexWrittenAt + 1);
        }

        private int GetRealIndex(int index)
        {
            return index + GetHeaderSizeOffset();
        }

        #region UserHeader
        //private UserHeader ReadUserHeader()
        //{
        //    int userHeaderSizeSize = UserHeader.GetSizeSize();
        //    byte[] userHeaderSizeBytes = new byte[userHeaderSizeSize];
        //    _storage.Seek(GetUserHeaderPosition());
        //    _storage.ReadByteArray(userHeaderSizeBytes);

        //    int userHeaderSize = UserHeader.GetSizeFromData(userHeaderSizeBytes);
        //    byte[] userHeaderBytes = new byte[userHeaderSize];
        //    _storage.ReadByteArray(userHeaderBytes);

        //    return UserHeader.Deserialize(userHeaderSizeBytes.Append(userHeaderBytes));
        //}

        public void PutUserHeaderSize(int userHeaderSize)
        {
            _storage.Seek(GetUserHeaderPosition());
            _storage.WriteByteArray(userHeaderSize.ToBytes());
        }

        public void PutUserHeader(byte[] userHeader)
        {
            int maxUserHeaderSize = GetUserHeaderSize();
            if (userHeader.Length > maxUserHeaderSize)
                throw new InvalidUserHeaderException("The user header size must be " + maxUserHeaderSize + " bytes long or less");
            _storage.Seek(GetUserHeaderPosition() + PersistenceConstants.IntSize);
            _storage.WriteByteArray(userHeader.ExtendTo(maxUserHeaderSize));
        }

        #endregion

        public int GetNextIndex()
        {
            return GetActualNextIndex() - GetHeaderSizeOffset();
        }

        private int GetHeaderSizeOffset()
        {
            return 2*PersistenceConstants.IntSize + GetUserHeaderSize();
        }
    }
}
