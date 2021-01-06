using System;
using System.Collections.Generic;
using System.Text;

namespace MB.Core.Application.Helper.Exceptions
{
    public class BaseServiceException : Exception
    {
        public BaseServiceException(string message) : base(message)
        {
        }
    }
}
