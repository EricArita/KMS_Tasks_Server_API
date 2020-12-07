using Core.Application.Helper;
using Core.Application.Models;
using Core.Domain.DbEntities;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public interface IAuthentication
    {
        Task<Response<ApplicationUser>> RegisterAsync(UserRegisterModel model);
        Task<Response<AuthResponseModel>> VerifyAccount(string userName, string password);
        Task<Response<AuthResponseModel>> HandleFacebookLoginAsync(string userAccessToken);
    }

    public abstract class JWTAuthenticationBase {
        protected abstract Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user);
    }
}
