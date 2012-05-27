using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PHT.Exceptions
{
    public class InvalidKeySizeException : Exception
    {
        public InvalidKeySizeException(string message) : base(message)
        {
        }
    }
}
