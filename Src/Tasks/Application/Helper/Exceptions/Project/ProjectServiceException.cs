using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Helper.Exceptions.Project
{
    public class ProjectServiceException : Exception
    {
        private string v;

        public ProjectServiceException(string v)
        {
            this.v = v;
        }
    }
}
