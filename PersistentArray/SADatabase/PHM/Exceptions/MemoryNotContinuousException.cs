using System;

namespace PHM.Exceptions
{
    public class MemoryNotContinuousException : Exception
    {
        public MemoryNotContinuousException(string message) : base(message)
        {
        }
    }
}
