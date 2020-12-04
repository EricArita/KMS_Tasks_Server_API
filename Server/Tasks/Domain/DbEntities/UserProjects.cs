using Core.Domain.Constants;

namespace Core.Domain.DbEntities
{
    public partial class UserProjects
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ProjectId { get; set; }
        public Enums.ProjectRoles RoleId { get; set; }

        public virtual Project Project { get; set; }
        public virtual ProjectRole ProjectRole { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
