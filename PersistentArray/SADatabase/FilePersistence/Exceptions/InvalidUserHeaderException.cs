using System;

namespace FilePersistence.Exceptions
{
    public class InvalidUserHeaderException : Exception
    {
        public InvalidUserHeaderException(string message) : base(message)
        {
        }
    }
}
