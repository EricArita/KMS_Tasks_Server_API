using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MB.Core.Application.Models.User
{
    public class FindUserByFieldsModel
    {
        [Required]
        public string UserNameOrEmail { get; set; }
    }
}
