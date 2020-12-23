using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Helper.Exceptions
{
    public class BaseServiceException : Exception
    {
        public BaseServiceException(string message) : base(message)
        {
        }
    }
}
