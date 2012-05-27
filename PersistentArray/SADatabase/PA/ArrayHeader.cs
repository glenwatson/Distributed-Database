//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using ByteHelper;
//using FilePersistence;
//using PA.Exceptions;
//using Persistence;

//namespace PA
//{
//    public class ArrayHeader
//    {
//        public int ElementSize { get; private set; }

//        public ArrayHeader(int elementSize)
//        {
//            if (elementSize <= 0)
//                throw new InvalidElementSizeException("The element size must be greater than 0");
//            ElementSize = elementSize;
//        }

//        public static int GetElementSizePosition()
//        {
//            return 0;
//        }

//        public byte[] Serialize()
//        {
//            //byte[] baseBytes = base.Serialize();
//            //byte[] serialized = ElementSize.ToBytes().Append(baseBytes);
//            //return serialized;
//            throw new NotImplementedException();
//        }

//        public ArrayHeader Deserialize(byte[] data)
//        {
//            //ElementSize
//            //storage.Seek(GetElementSizePosition());
//            //int eleSize = storage.ReadInt();
//            throw new NotImplementedException();
//        }
//    }
//}
