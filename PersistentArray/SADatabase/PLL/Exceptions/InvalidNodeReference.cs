using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PLL.Exceptions
{
    class InvalidNodeReference : Exception
    {
        public InvalidNodeReference(string message) : base(message)
        {
        }
    }
}
