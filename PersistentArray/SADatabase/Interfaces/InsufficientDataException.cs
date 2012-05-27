using System;

namespace Interfaces
{
    public class InsufficientDataException : Exception
    {
        public InsufficientDataException(string message) : base(message)
        {
        }
    }
}
