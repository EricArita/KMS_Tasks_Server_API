using Core.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Models.Task
{
    public class UpdateTaskInfoModel 
    {
        public string Name { get; set; }
        public DateTime? Schedule { get; set; }
        public string ScheduleString { get; set; }
        public Enums.TaskPriorityLevel? PriorityId { get; set; }
        public long? ProjectId { get; set; }
        public long? ParentId { get; set; }
        public DateTime? ReminderSchedule { get; set; }
        public bool Reminder { get; set; }
        public long? AssignedBy { get; set; }
        public long? AssignedFor { get; set; }
        public long? CreatedBy { get; set; }
    }
}
