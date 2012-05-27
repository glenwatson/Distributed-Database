//using System;
//using ByteHelper;
//using FilePersistence.Exceptions;
//using Interfaces;
//using Persistence;

//namespace FilePersistence
//{
//    public class Header : ISerializeable
//    {
//        public int NextIndex { get; private set; }
//        public UserHeader UserHeader { get; private set; }

//        public int UserHeaderSize
//        {
//            get { return UserHeader.Size; }
//        }

//        public virtual int HeaderSize
//        {
//            get { return 2*4 + UserHeaderSize; }
//        }

//        public Header(int nextIdx, int userHeaderSize)
//        {
            
//            if (nextIdx < 0)
//                throw new InvalidNextIndexException("The next index must be positive");
//            NextIndex = nextIdx;
//            UserHeader = new UserHeader(userHeaderSize);
//        }

//        public void SetNextIndex(int index, IStorage storage)
//        {
//            NextIndex = index;
//            storage.Seek(GetNextIndexPosition());
//            storage.WriteInt(index);
//        }

//        public void SetUserHeader(byte[] newUserHeader)
//        {
//            UserHeader.Data = newUserHeader;
//        }

//        public byte[] Serialize()
//        {
//            byte[] headerbytes = NextIndex.ToBytes().Append(UserHeader.Serialize());
//            return headerbytes;
//        }

//        public static Header Deserialize(byte[] data)
//        {
//            //Next Index
//            int nextIdx =
//                data.SubArray(GetNextIndexPosition(), GetNextIndexPosition() + PersistenceConstants.IntSize).ToInt();

//            //UserHeaderSize
//            int userHeaderSize = data.SubArray(GetUserHeaderPosition(), GetUserHeaderPosition() + GetUserHeaderSizeSize()).ToInt();

//            //UserHeader
//            byte[] userHeader = data.SubArray(GetUserHeaderPosition() + PersistenceConstants.IntSize, data.Length);
//            UserHeader uh = new UserHeader(userHeaderSize) { Data = userHeader };

//            return new Header(nextIdx, userHeaderSize) { UserHeader = uh };
//        }

//        public static int GetUserHeaderSizeSize()
//        {
//            return UserHeader.GetSizeSize();
//        }

//        public static int GetHeaderSizeFromData(byte[] data)
//        {
//            return UserHeader.GetSizeFromData(data);
//        }

//        #region GetPositions
//        public static int GetNextIndexPosition()
//        {
//            return 0;
//        }

//        public static int GetUserHeaderPosition()
//        {
//            return GetNextIndexPosition()+PersistenceConstants.IntSize;
//        }
//        #endregion

//    }
//}
