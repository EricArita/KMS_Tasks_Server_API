using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace MB.Core.Application.Helper.Exceptions.User
{
    public class UserServiceException : BaseServiceException
    {
        public UserServiceException(string message) : base(message)
        {
        }

        public List<IdentityError> IdentityErrors { get; set; }

        public UserServiceException(string message, List<IdentityError> identityErrors) : base (message)
        {
            IdentityErrors = identityErrors;
        }
    }
}
