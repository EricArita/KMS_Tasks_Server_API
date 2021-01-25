using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using UM.Core.Application.Helper;
using UM.Core.Application.Models;
using UM.Core.Domain.DbEntities;

namespace UM.Core.Application.Interfaces
{
    public interface IUserManagement
    {
        Task<HttpResponse<ApplicationUser>> RegisterAsync(UserRegisterModel model);
        Task<HttpResponse<ApplicationUser>> GetUserByUsername(string username);
        Task<IList<string>> GetUserRoleAsync(ApplicationUser user);
        Task<IList<Claim>> GetUserClaimAsync(ApplicationUser user);
    }
}
