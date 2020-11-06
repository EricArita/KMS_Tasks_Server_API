using System;
using System.Collections.Generic;

namespace Core.Domain.DbEntities
{
    public partial class PriorityLevel
    {
        public PriorityLevel()
        {
            Tasks = new HashSet<Tasks>();
        }

        public int Id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Tasks> Tasks { get; set; }
    }
}
