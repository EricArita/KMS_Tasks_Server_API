using Core.Application.Helper;
using Core.Application.Models;
using Core.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public abstract class IAuthentication
    {
        public abstract Task<Response<AuthenticationResponseModel>> VerifyAccount(string userName, string password);
        protected abstract Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user);
    }
}
