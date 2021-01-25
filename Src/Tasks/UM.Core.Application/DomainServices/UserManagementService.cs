using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using UM.Core.Application.Helper;
using UM.Core.Application.Interfaces;
using UM.Core.Application.Models;
using UM.Core.Domain.Constants;
using UM.Core.Domain.DbEntities;

namespace UM.Core.Application.DomainServices
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

        public async Task<HttpResponse<ApplicationUser>> GetUserByUsername(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName) ?? await _userManager.FindByEmailAsync(userName);
            if (user != null)
                return new HttpResponse<ApplicationUser>(true, user, "success");

            return new HttpResponse<ApplicationUser>(false, null, $"Does not exist any user with this username {userName}");
        }

        public async Task<IList<string>> GetUserRoleAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<IList<Claim>> GetUserClaimAsync(ApplicationUser user)
        {
            return await _userManager.GetClaimsAsync(user);
        }
    }
}