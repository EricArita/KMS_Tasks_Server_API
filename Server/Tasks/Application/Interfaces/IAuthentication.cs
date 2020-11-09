using Core.Application.Helper;
using Core.Application.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public interface IAuthentication
    {
        Task<Response<ApplicationUser>> RegisterAsync(RegisterModel model);
        Task<Response<AuthResponseModel>> VerifyAccount(string userName, string password);
    }

    public abstract class GenerateJWTAuthentication
    {
        protected virtual Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
        {
            return null;
        }
    }
}
