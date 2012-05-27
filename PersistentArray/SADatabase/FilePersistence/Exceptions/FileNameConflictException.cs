using System;

namespace FilePersistence.Exceptions
{
    public class FileNameConflictException : Exception
    {
        public FileNameConflictException(string message) : base(message)
        {
        }
    }
}
