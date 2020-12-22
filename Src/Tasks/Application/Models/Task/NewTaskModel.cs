using Core.Domain.Constants;
using System;
using System.ComponentModel.DataAnnotations;

namespace Core.Application.Models
{
    public class NewTaskModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Task's {0} is required")]
        public string Name { get; set; }
        public DateTime? Schedule { get; set; }
        public string ScheduleString { get; set; }
        [EnumDataType(typeof(Enums.TaskPriorityLevel))]
        public Enums.TaskPriorityLevel? PriorityId { get; set; }
        [Required(ErrorMessage = "Task must belong to one {0}")]
        public long? ProjectId { get; set; }
        public long? ParentId { get; set; }
        public bool Reminder { get; set; }
        public DateTime? ReminderSchedule { get; set; }
        public long? AssignedBy { get; set; }
        public long? AssignedFor { get; set; }
    }
}
