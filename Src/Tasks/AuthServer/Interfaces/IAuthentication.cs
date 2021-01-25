using AuthServer.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace AuthServer.Interfaces
{
    public interface IAuthentication
    {
        Task<HttpResponse<AuthResponseModel>> VerifyAccount(string userName, string password);
        Task<HttpResponse<AuthResponseModel>> RefreshTokenAsync(string token);
    }

    public abstract class JWTAuthenticationBase {
        protected abstract Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user);
    }
}
