using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PA.Exceptions
{
    public class InvalidElementSizeException : Exception
    {
        public InvalidElementSizeException(string message)
            : base(message)
        {

        }
    }
}
