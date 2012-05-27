using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ByteHelper.Exceptions
{
    public class InvalidRangeException : Exception
    {
        public InvalidRangeException(string message) : base(message)
        {
        }
    }
}
