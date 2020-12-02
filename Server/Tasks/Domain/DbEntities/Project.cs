using System;
using System.Collections.Generic;

namespace Core.Domain.DbEntities
{
    public partial class Project
    {
        public Project()
        {
            Tasks = new HashSet<Tasks>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? ParentId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedBy { get; set; }
        public bool Deleted { get; set; }

        public virtual Project Parent { get; set; }
        public virtual ApplicationUser CreatedByUser { get; set; }
        public virtual ApplicationUser UpdatedByUser { get; set; }
        public virtual ICollection<Tasks> Tasks { get; set; }
        public virtual ICollection<Project> Children { get; set; }
    }
}
