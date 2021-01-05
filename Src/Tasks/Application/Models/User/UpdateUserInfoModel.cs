using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Application.Models.User
{
    public class UpdateUserInfoModel
    {
        [MinLength(1, ErrorMessage = "First name must be at least more than 1 character")]
        public string FirstName { get; set; }
        public string MidName { get; set; }
        [MinLength(1, ErrorMessage = "Last name must be at least more than 1 character")]
        public string LastName { get; set; }

        [MinLength(10, ErrorMessage = "Mobile number must have more than or equals to 10 characters")]
        public string Phone { get; set; }
        [MinLength(6, ErrorMessage = "Current password must be longer than or equals to 6 characters")]
        public string CurrentPassword { get; set; }
        [MinLength(6, ErrorMessage = "New password must be longer than or equals to 6 characters")]
        public string NewPassword { get; set; }
        [MinLength(1, ErrorMessage = "Avatar link must be at least more than 1 character")]
        public string AvatarUrl { get; set; }
        public string Address { get; set; }
    }
}
