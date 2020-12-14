using Core.Application.Models;
using Core.Domain.Constants;
using Core.Domain.DbEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
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

            modelBuilder.Entity<ApplicationUser>(entity => {
                entity.Property(e => e.UserId).UseIdentityColumn();
            });

            modelBuilder.Entity<ProjectRole>(entity =>
            {
                entity.Property(e => e.Id).HasConversion<int>();

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PriorityLevel>(entity =>
            {
                entity.Property(e => e.Id).HasConversion<int>();

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);
                
                entity.HasOne(d => d.Parent)
                    .WithMany()
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Project_HaveParentProject");

                entity.HasOne(p => p.CreatedByUser)
                    .WithMany()
                    .HasPrincipalKey(u => u.UserId)
                    .HasForeignKey(p => p.CreatedBy)
                    .HasConstraintName("FK_Project_CreatedBy_User");

                entity.HasOne(p => p.UpdatedByUser)
                    .WithMany()
                    .HasPrincipalKey(u => u.UserId)
                    .HasForeignKey(p => p.UpdatedBy)
                    .HasConstraintName("FK_Project_UpdatedBy_User");

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedOnAdd();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ReminderSchedule).HasColumnType("datetime");

                entity.Property(e => e.Schedule).HasColumnType("datetime");

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(t => t.PriorityId).HasConversion<int>();

                entity.HasOne(d => d.Parent)
                    .WithMany()
                    .HasForeignKey(d => d.ParentId)
                    .HasConstraintName("FK_Tasks_HaveParentTasks");

                entity.HasOne(d => d.Priority)
                    .WithMany()
                    .HasForeignKey(d => d.PriorityId)
                    .HasConstraintName("FK_Tasks_PriorityLevel");

                entity.HasOne(d => d.Project)
                    .WithMany()
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_Tasks_Project");

                entity.HasOne(t => t.CreatedByUser)
                   .WithMany()
                   .HasPrincipalKey(u => u.UserId)
                   .HasForeignKey(t => t.CreatedBy)
                   .HasConstraintName("FK_Task_CreatedBy_User");

                entity.HasOne(t => t.AssignedByUser)
                   .WithMany()
                   .HasPrincipalKey(u => u.UserId)
                   .HasForeignKey(t => t.AssignedBy)
                   .HasConstraintName("FK_Task_AssignedBy_User");

                entity.HasOne(t => t.AssignedForUser)
                   .WithMany()
                   .HasPrincipalKey(u => u.UserId)
                   .HasForeignKey(t => t.AssignedFor)
                   .HasConstraintName("FK_Task_AssignedFor_User");
            });

            modelBuilder.Entity<UserProjects>(entity =>
            {
                entity.HasKey(item => new { item.UserId, item.ProjectId, item.RoleId });
                
                entity.HasOne(d => d.Project)
                    .WithMany()
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProjects_Project");

                entity.Property(item => item.RoleId).HasConversion<int>();

                entity.HasOne(d => d.ProjectRole)
                   .WithMany()
                   .HasForeignKey(d => d.RoleId)
                   .OnDelete(DeleteBehavior.ClientSetNull)
                   .HasConstraintName("FK_UserProjects_ProjectRole");

                entity.HasOne(item => item.User)
                   .WithMany()
                   .HasPrincipalKey(u => u.UserId)
                   .HasForeignKey(item => item.UserId)
                   .HasConstraintName("FK_UserProjects_BelongsTo_User");
            });

            // Populate with data
            modelBuilder.Entity<ProjectRole>().HasData(
                    Enum.GetValues(typeof(Enums.ProjectRoles))
                        .Cast<Enums.ProjectRoles>()
                        .Where(e => e != 0)
                        .Select(e => {
                            return new ProjectRole()
                            {
                                Id = e,
                                Name = e.ToString()
                            };
                        })
                );

           modelBuilder.Entity<PriorityLevel>().HasData(
                    Enum.GetValues(typeof(Enums.TaskPriorityLevel))
                        .Cast<Enums.TaskPriorityLevel>()
                        .Where(e => e != 0)
                        .Select(e =>
                        {
                            return new PriorityLevel()
                            {
                                Id = e,
                                DisplayName = e.ToString()
                            };
                        }));

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
