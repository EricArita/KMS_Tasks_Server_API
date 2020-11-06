using Core.Domain.DbEntities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Contexts
{
    public partial class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<PriorityLevel> PriorityLevel { get; set; }
        public virtual DbSet<Project> Project { get; set; }
        public virtual DbSet<Sections> Sections { get; set; }
        public virtual DbSet<SysLogs> SysLogs { get; set; }
        public virtual DbSet<Tasks> Tasks { get; set; }
        public virtual DbSet<UserProjects> UserProjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {         
            modelBuilder.Entity<PriorityLevel>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.DisplayName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Project>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");
            });

            modelBuilder.Entity<Sections>(entity =>
            {
                entity.Property(e => e.CreatedAt).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UpdatedAt).HasColumnType("datetime");

                entity.HasOne(d => d.Project)
                    .WithMany(p => p.Sections)
                    .HasForeignKey(d => d.ProjectId)
                    .HasConstraintName("FK_Sections_Project");
            });

            modelBuilder.Entity<SysLogs>(entity =>
            {
                entity.Property(e => e.Exception).IsRequired();

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Logger).IsRequired();

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.Trace).IsRequired();

                entity.Property(e => e.When).HasColumnType("datetime");
            });

            modelBuilder.Entity<Tasks>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ReminderSchedule).HasColumnType("datetime");

                entity.Property(e => e.Schedule).HasColumnType("datetime");

                entity.Property(e => e.ScheduleString)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdatedDate).HasColumnType("datetime");

                entity.HasOne(d => d.AssignedByNavigation)
                    .WithMany(p => p.TasksAssignedByNavigation)
                    .HasPrincipalKey(p => p.UserId)
                    .HasForeignKey(d => d.AssignedBy)
                    .HasConstraintName("FK_Tasks_AspNetUsers");

                entity.HasOne(d => d.AssignedForNavigation)
                    .WithMany(p => p.TasksAssignedForNavigation)
                    .HasPrincipalKey(p => p.UserId)
                    .HasForeignKey(d => d.AssignedFor)
                    .HasConstraintName("FK_Tasks_AspNetUsers1");

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.TasksCreatedByNavigation)
                    .HasPrincipalKey(p => p.UserId)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Tasks_AspNetUsers2");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.InverseParent)
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

                entity.HasOne(d => d.Section)
                    .WithMany(p => p.Tasks)
                    .HasForeignKey(d => d.SectionId)
                    .HasConstraintName("FK_Tasks_Sections");
            });

            modelBuilder.Entity<UserProjects>(entity =>
            {
                entity.HasNoKey();

                entity.HasOne(d => d.Project)
                    .WithMany()
                    .HasForeignKey(d => d.ProjectId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProjects_Project1");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasPrincipalKey(p => p.UserId)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserProjects_AspNetUsers1");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}