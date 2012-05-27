using System;

namespace FilePersistence.Exceptions
{
    class InvalidNextIndexException : Exception
    {
        public InvalidNextIndexException(string message) : base(message)
        {
        }
    }
}
