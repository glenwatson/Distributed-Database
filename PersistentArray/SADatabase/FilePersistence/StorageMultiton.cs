using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Interfaces;
using Persistence;

namespace FilePersistence
{
    public static class StorageMultiton
    {
        private static readonly Dictionary<string, FileStream> Instances = new Dictionary<string, FileStream>();

        public static IStorage GetInstance(string filePath)
        {
            lock (Instances)
            {
                FileStream instance;
                if (!Instances.TryGetValue(filePath, out instance))
                {
                    instance = File.Open(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
                    Instances.Add(filePath, instance);
                }
                return new RandomAccessFile(instance);
            }
        }

        public static void RemoveInstance(string filePath)
        {
            lock (Instances)
            {
                Instances.Remove(filePath);
            }
        }
    }
}
