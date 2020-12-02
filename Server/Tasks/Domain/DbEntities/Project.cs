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

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long? ParentId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public bool Deleted { get; set; }

        public virtual Project Parent { get; set; }
        public virtual ICollection<Tasks> Tasks { get; set; }
        public virtual ICollection<Project> Children { get; set; }
    }
}
