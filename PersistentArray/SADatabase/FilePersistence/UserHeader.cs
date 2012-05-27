using ByteHelper;
using FilePersistence.Exceptions;
using Interfaces;
using Persistence;

namespace FilePersistence
{
    public class UserHeader : ISerializeable
    {
        private byte[] _data;
        public byte[] Data
        {
            get { return _data; }
            set
            {
                if (value.Length > Size)
                    throw new InvalidUserHeaderException("User header is too big to fit in " + Size + " bytes");
                _data = value.ExtendTo(Size);
            }
        }
        public int Size { get; private set; }

        public UserHeader(int size)
        {
            if (size < 0)
                throw new InvalidUserHeaderException("The user header size must be positive");
            Size = size;
            Data = new byte[size];
        }

        #region Seek

        public static int GetUserHeaderSizePosition()
        {
            return 0;
        }

        public static int GetUserHeaderPosition()
        {
            return GetUserHeaderSizePosition() + PersistenceConstants.IntSize;
        }
        #endregion

        public static int GetSizeSize()
        {
            return PersistenceConstants.IntSize;
        }

        public static int GetSizeFromData(byte[] data)
        {
            if (data.Length < GetSizeSize())
                throw new InsufficientDataException("Not enough data to deserialize");
            return data.ToInt();
        }

        public byte[] Serialize()
        {
            byte[] serializeBytes = Size.ToBytes().Append(_data);
            return serializeBytes;
        }

        public static UserHeader Deserialize(byte[] data)
        {
            int size = GetSizeFromData(data);
            byte[] userHeaderData = data.SubArray(GetSizeSize(), data.Length);
            UserHeader uHeader = new UserHeader(size) { Data = userHeaderData };

            return uHeader;
        }
    }
}
