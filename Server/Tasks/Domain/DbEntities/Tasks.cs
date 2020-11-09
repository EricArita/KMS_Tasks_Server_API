using System;
using System.Collections.Generic;

namespace Core.Domain.DbEntities
{
    public partial class Tasks
    {
        public Tasks()
        {
            InverseParent = new HashSet<Tasks>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? Schedule { get; set; }
        public string ScheduleString { get; set; }
        public int? PriorityId { get; set; }
        public bool Deleted { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? ProjectId { get; set; }
        public int? SectionId { get; set; }
        public int? ParentId { get; set; }
        public DateTime? ReminderSchedule { get; set; }
        public bool Reminder { get; set; }
        public int? AssignedBy { get; set; }
        public int? AssignedFor { get; set; }
        public int? CreatedBy { get; set; }

        public virtual Tasks Parent { get; set; }
        public virtual PriorityLevel Priority { get; set; }
        public virtual Project Project { get; set; }
        public virtual Sections Section { get; set; }
        public virtual ICollection<Tasks> InverseParent { get; set; }
    }
}
