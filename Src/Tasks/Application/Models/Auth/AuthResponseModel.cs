using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MB.Core.Application.Models
{
    public class AuthResponseModel
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Avatar { get; set; }
        public IList<string> Roles { get; set; }
        public string Token { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpiration { get; set; }
    }
}
