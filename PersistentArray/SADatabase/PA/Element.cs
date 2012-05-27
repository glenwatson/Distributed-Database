using System;
using Interfaces;
using PA.Exceptions;

namespace PA
{
    public class Element : ISerializeable
    {
        public byte[] Data { get; set; }

        public Element(byte[] data)
        {
            Data = data;
        }

        public byte[] Serialize()
        {
            return Data;
        }

        //public void WriteToStorage(IStorage storage, int elementSize)
        //{
        //    if (Data.Length > elementSize)
        //        throw new ParameterException("The element data is too big to fit in " + elementSize + " bytes",
        //                                     "elementSize");
        //    byte[] buffer = new byte[elementSize];
        //    Data.CopyInto(buffer);
        //    storage.WriteByteArray(buffer);
        //}

        public static Element ReadFromStorage(IStorage storage, int elementSize)
        {
            byte[] data = new byte[elementSize];
            storage.ReadByteArray(data);
            return new Element(data);
        }
    }
}
