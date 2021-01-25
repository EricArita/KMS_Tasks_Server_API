using MB.Core.Application.DTOs;
using MB.Core.Domain.DbEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MB.Core.Application.Models.User
{
    public class UserResponseModel : UserDTO
    {
        public UserResponseModel(ApplicationUser user) : base(user)
        {

        }
    }
}
