using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PHT.Exceptions
{
    public class InvalidValueSizeException : Exception
    {
        public InvalidValueSizeException(string message) : base(message)
        {
        }
    }
}
