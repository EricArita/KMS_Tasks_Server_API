using MB.Core.Application.Helper;
using MB.Core.Application.Models;
using MB.Core.Domain.DbEntities;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace MB.Core.Application.Interfaces
{
    public interface IAuthentication
    {
        Task<HttpResponse<ApplicationUser>> RegisterAsync(UserRegisterModel model);
        Task<HttpResponse<AuthResponseModel>> VerifyAccount(string userName, string password);
        Task<HttpResponse<AuthResponseModel>> HandleFacebookLoginAsync(string userAccessToken);
        Task<HttpResponse<AuthResponseModel>> RefreshTokenAsync(string token);
    }

    public abstract class JWTAuthenticationBase {
        protected abstract Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user);
    }
}
