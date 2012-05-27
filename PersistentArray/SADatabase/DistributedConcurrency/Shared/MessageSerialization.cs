using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization.Formatters.Soap;
using System.Text;
using ByteHelper;
using DistributedConcurrency.Shared.Communication.Messages;

namespace DistributedConcurrency.Shared
{
    class MessageSerialization
    {
        private static readonly BinaryFormatter _formatter = new BinaryFormatter();
        //private static readonly SoapFormatter _formatter = new SoapFormatter();

        public static byte[] Serialize(BaseMessage msg)
        {
            MemoryStream memoryStream = new MemoryStream();
            _formatter.Serialize(memoryStream, msg);
            byte[] serializedObj = memoryStream.ToArray();

            //Console.WriteLine(Encoding.ASCII.GetString(serializedObj));

            return serializedObj;
        }

        public static int GetMessageSize(byte[] sizeBytes)
        {
            return sizeBytes.ToInt();
        }

        public static BaseMessage Deserialze(byte[] serializedMessage)
        {
            //Console.WriteLine(Encoding.ASCII.GetString(serializedMessage.SubArray(2, serializedMessage.Length)));

            MemoryStream inStream = new MemoryStream();
            inStream.Write(serializedMessage, 0, serializedMessage.Length);
            inStream.Seek(0, SeekOrigin.Begin);
            BaseMessage deserialized = (BaseMessage) _formatter.Deserialize(inStream);
            return deserialized;
        }
    }
}
