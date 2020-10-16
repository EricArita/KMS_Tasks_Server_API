using System.Collections.Generic;

namespace Core.Application.Models
{
    public class AuthenticationResponseModel
    {
        public bool IsAuthenticated { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IList<string> Roles { get; set; }
        public string Token { get; set; }
    }
}
