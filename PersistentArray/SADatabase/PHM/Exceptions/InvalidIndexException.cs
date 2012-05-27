using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PHM.Exceptions
{
    public class InvalidIndexException : Exception
    {
        public InvalidIndexException(string message) : base(message)
        {
        }
    }
}
