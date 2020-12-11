using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Helper.Exceptions
{
    public class BaseServiceException : Exception
    {
       public int StatusCode { get; private set; }

        public BaseServiceException(int statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
