using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace DistributedConcurrency.DM.Journaling
{
    class Serializer<T> //Singleton
    {
        private static BinaryFormatter _formatter;

        private static BinaryFormatter GetFormatter()
        {
            if (_formatter == null)
                _formatter = new BinaryFormatter();
            return _formatter;
        }
        public static void Serialize(T t, Stream stream)
        {
            GetFormatter().Serialize(stream, t);
        }
	    public static T Deserialize(Stream stream)
        {
            return (T)GetFormatter().Deserialize(stream);
        }
    }
}
