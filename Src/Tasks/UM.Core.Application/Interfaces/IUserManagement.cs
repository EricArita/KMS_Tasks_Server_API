using System.Threading.Tasks;
using UM.Core.Application.Helper;
using UM.Core.Application.Models;
using UM.Core.Domain.DbEntities;

namespace UM.Core.Application.Interfaces
{
    public interface IUserManagement
    {
        Task<HttpResponse<ApplicationUser>> RegisterAsync(UserRegisterModel model);
        Task<HttpResponse<ApplicationUser>> GetUserByEmail(string email);
    }
}
