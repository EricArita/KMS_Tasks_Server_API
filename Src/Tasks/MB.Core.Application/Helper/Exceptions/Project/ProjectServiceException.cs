using System;
using System.Collections.Generic;
using System.Text;

namespace MB.Core.Application.Helper.Exceptions.Project
{
    public class ProjectServiceException : BaseServiceException
    {
        public ProjectServiceException(string message) : base(message)
        {
        }
    }
}
