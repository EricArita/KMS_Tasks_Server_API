using Core.Application.DTOs;
using Core.Domain.DbEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Models.User
{
    public class UserResponseModel : UserDTO
    {
        public UserResponseModel(ApplicationUser user) : base(user)
        {

        }
    }
}
