using System;
using System.Collections.Generic;

namespace Core.Domain.DbEntities
{
    public partial class Project
    {
        public Project()
        {
            Sections = new HashSet<Sections>();
            Tasks = new HashSet<Tasks>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool Deleted { get; set; }

        public virtual ICollection<Sections> Sections { get; set; }
        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
