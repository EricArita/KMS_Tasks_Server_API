using Core.Application.Models;
using Core.Domain.Constants;
using Core.Domain.DbEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Contexts
{
    public partial class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
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

        public virtual DbSet<PriorityLevel> PriorityLevel { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<UserProjects> UserProjects { get; set; }
        public virtual DbSet<ProjectRole> ProjectRoles { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ProjectRole>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PriorityLevel>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.Children)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Project_Project");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ReminderSchedule).HasColumnType("datetime");

                entity.Property(e => e.Schedule).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.Children)
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Tasks_Tasks");

                entity.HasOne(d => d.Priority)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.PriorityId)
                    .HasConstraintName("FK_Tasks_PriorityLevel");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_Tasks_Project");
            });

            modelBuilder.Entity<UserProjects>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Project)
                    .WithMany()
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProjects_Project");

                entity.HasOne(d => d.ProjectRole)
                   .WithMany()
                   .HasForeignKey(d => d.RoleId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_UserProjects_ProjectRole");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
