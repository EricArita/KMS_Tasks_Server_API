using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models
{
    public class AuthRequestModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
