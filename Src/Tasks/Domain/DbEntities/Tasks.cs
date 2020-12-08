using Core.Domain.Constants;
using System;
using System.Collections.Generic;

namespace Core.Domain.DbEntities
{
    public partial class Tasks
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? Schedule { get; set; }
        public string ScheduleString { get; set; }
        public Enums.TaskPriorityLevel? PriorityId { get; set; }
        public bool Deleted { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public long? ProjectId { get; set; }
        public long? ParentId { get; set; }
        public DateTime? ReminderSchedule { get; set; }
        public bool Reminder { get; set; }
        public long? AssignedBy { get; set; }
        public long? AssignedFor { get; set; }
        public long? CreatedBy { get; set; }

        public virtual Tasks Parent { get; set; }
        public virtual PriorityLevel Priority { get; set; }
        public virtual Project Project { get; set; }
        public virtual ApplicationUser AssignedByUser { get; set; }
        public virtual ApplicationUser AssignedForUser { get; set; }
        public virtual ApplicationUser CreatedByUser { get; set; }
    }
}
