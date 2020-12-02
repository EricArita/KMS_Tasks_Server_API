namespace Core.Domain.DbEntities
{
    public partial class UserProjects
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }
        public byte RoleId { get; set; }

        public virtual Project Project { get; set; }
        public virtual ProjectRole ProjectRole { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}
