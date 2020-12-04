using System;

namespace Core.Application.Models
{
    public class NewTaskModel
    {
        public string Name { get; set; }
        public DateTime? Schedule { get; set; }
        public string ScheduleString { get; set; }
        public int? PriorityId { get; set; }
        public int? ProjectId { get; set; }
        public int? SectionId { get; set; }
        public int? ParentId { get; set; }
        public bool Reminder { get; set; }
        public DateTime? ReminderSchedule { get; set; }
        public int? AssignedBy { get; set; }
        public int? AssignedFor { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
