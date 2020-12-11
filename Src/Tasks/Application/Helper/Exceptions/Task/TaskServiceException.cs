using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Helper.Exceptions.Task
{
    public class TaskServiceException : BaseServiceException
    {
        public TaskServiceException(int statusCode, string message) : base(statusCode, message)
        {
        }
    }
}
