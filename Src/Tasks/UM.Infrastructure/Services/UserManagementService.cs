using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using UM.Core.Application.Helper;
using UM.Core.Application.Interfaces;
using UM.Core.Application.Models;
using UM.Core.Domain.Constants;
using UM.Core.Domain.DbEntities;

namespace UM.Infrastructure.Services
{
    public class UserManagementService : IUserManagement

    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<HttpResponse<ApplicationUser>> RegisterAsync(UserRegisterModel model)
        {
            var userWithSameEmail = await _userManager.FindByEmailAsync(model.Email);
            var res = new HttpResponse<ApplicationUser>();

            if (userWithSameEmail == null)
            {
                var newUser = new ApplicationUser
                {
                    UserName = model.Username,
                    Email = model.Email,
                    PhoneNumber = model.Phone,
                    FirstName = model.FirstName,
                    MidName = model.MidName,
                    LastName = model.LastName,
                    Status = 1
                };

                var result = await _userManager.CreateAsync(newUser, model.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, Enums.UserRoles.User.ToString());
                    res.OK = true;
                    res.Message = $"Register successfully";
                    res.Data = newUser;
                }
                else
                {
                    res.OK = false;
                    res.Message = $"Register fail";
                    res.Errors = result.Errors.ToList();
                }
            }
            else
            {
                res.OK = true;
                res.Message = $"Email or Username {model.Email } is already registered.";
            }

            return res;
        }
    
        public async Task<HttpResponse<ApplicationUser>> GetUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user != null) 
                return new HttpResponse<ApplicationUser>(true, user, "success");

            return new HttpResponse<ApplicationUser>(false, null, $"Does not exist any user with this {email}");
        }
    }
}