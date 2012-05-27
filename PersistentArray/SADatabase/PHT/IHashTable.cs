using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;

namespace PHT
{
    public interface IHashTable : IUserHeader, ICloseable
    {
        void Put(byte[] key, byte[] value);

        byte[] Get(byte[] key);

        void Remove(byte[] key);

    }
}
