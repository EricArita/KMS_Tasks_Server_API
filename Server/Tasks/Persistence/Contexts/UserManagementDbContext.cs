using Core.Domain.Constants;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contexts
{
    public class UserManagementDbContext : IdentityDbContext<ApplicationUser>
    {
        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options) : base(options)
        {
        }

        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (roleManager.Roles.Count() == 0)
            {
                //Seed Roles
                await roleManager.CreateAsync(new IdentityRole(Enums.UserRoles.Administrator.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Enums.UserRoles.Moderator.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Enums.UserRoles.User.ToString()));
            }
           
            if (!userManager.Users.Any(u => u.UserName == Constants.DefaultUsername))
            {
                //Seed Default User
                var defaultUser = new ApplicationUser
                {
                    UserName = Constants.DefaultUsername,
                    FirstName = "Bùi",
                    MidName = "Phan",
                    LastName = "Thọ",
                    Email = Constants.DefaultEmail,
                    EmailConfirmed = true,
                    PhoneNumber = "0349004909",
                    PhoneNumberConfirmed = true,
                    Active = true
                };

                await userManager.CreateAsync(defaultUser, Constants.DefaultPassword);
                await userManager.AddToRoleAsync(defaultUser, Constants.DefaultRole.ToString());
            } 
        }

        public async Task<int> SaveChanges()
        {
            return await base.SaveChangesAsync();
        }
    }
}
