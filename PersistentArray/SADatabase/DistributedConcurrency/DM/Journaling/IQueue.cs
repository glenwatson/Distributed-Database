using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DistributedConcurrency.DM.Journaling
{
    interface IQueue<T>
    {
        void Push(T t);
        T Peek();
	    void Pop();
    }
}
