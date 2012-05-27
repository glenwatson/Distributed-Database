using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Interfaces;

namespace PLL
{
    public interface ILinkedList : ICloseable, IUserHeader
    {

        int AddToStart(byte[] buffer);
        
        int AddToEnd(byte[] buffer);

        int AddBefore(int nodeReference, byte[] buffer);

        int AddAfter(int nodeReference, byte[] buffer);

        void Update(int nodeReference, byte[] buffer);

        byte[] Remove(int nodeReference);

        int Length();

        int GetElementSize();

        byte[] GetData(int nodeReference);

        int GetFirst();

        int GetLast();

        int GetNext(int nodeReference);

        int GetPrevious(int nodeReference);
    }
}
