using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;

namespace PHM
{
    public interface IHeap : ICloseable, IUserHeader
    {
        int Allocate(int allocationSize);

        void Free(int token);
    }
}
