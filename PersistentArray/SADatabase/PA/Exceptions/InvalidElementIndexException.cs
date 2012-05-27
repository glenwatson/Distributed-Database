using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.Exceptions
{
    public class InvalidElementIndexException : Exception
    {
        public InvalidElementIndexException(string message) : base(message)
        {
        }
    }
}
