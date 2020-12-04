using Core.Domain.Constants;

namespace Core.Domain.DbEntities
{
    public class ProjectRole
    {
        public Enums.ProjectRoles Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
