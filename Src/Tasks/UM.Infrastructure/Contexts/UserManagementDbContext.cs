using UM.Core.Domain.Constants;
using UM.Core.Domain.DbEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace UM.Infrastructure.Contexts
{
    public partial class UserManagementDbContext : IdentityDbContext<ApplicationUser>
    {
        public UserManagementDbContext()
        {
        }

        public UserManagementDbContext(DbContextOptions<UserManagementDbContext> options)
            : base(options)
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

            if (!userManager.Users.Any(u => u.UserName == DefaultUserConstants.DefaultUsername))
            {
                //Seed Default User
                var defaultUser = new ApplicationUser
                {
                    UserName = DefaultUserConstants.DefaultUsername,
                    FirstName = "Bùi",
                    MidName = "Phan",
                    LastName = "Thọ",
                    Email = DefaultUserConstants.DefaultEmail,
                    EmailConfirmed = true,
                    PhoneNumber = "0349004909",
                    PhoneNumberConfirmed = true,
                    Status = 1
                };

                await userManager.CreateAsync(defaultUser, DefaultUserConstants.DefaultPassword);
                await userManager.AddToRoleAsync(defaultUser, DefaultUserConstants.DefaultRole.ToString());
            }
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>(entity => {
                entity.Property(e => e.UserId).UseIdentityColumn();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
