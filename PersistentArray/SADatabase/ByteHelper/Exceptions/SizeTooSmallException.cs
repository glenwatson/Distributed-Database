using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ByteHelper.Exceptions
{
    public class SizeTooSmallException : Exception
    {
        public SizeTooSmallException(string message) : base(message)
        {
        }
    }
}
