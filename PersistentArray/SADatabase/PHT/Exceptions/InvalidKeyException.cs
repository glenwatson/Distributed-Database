using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PHT.Exceptions
{
    public class InvalidKeyException : Exception
    {
        public InvalidKeyException(string message) : base(message)
        {
        }
    }
}
