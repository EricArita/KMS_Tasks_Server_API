using Core.Application.Models.Project;
using Core.Domain.DbEntities;
using Infrastructure.Persistence.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Models.Task
{
    public class TaskResponseModel
    {
        public TaskResponseModel(Tasks coreTask)
        {
            Id = coreTask.Id;
            Name = coreTask.Name;
            CreatedDate = coreTask.CreatedDate;
            Schedule = coreTask.Schedule;
            ScheduleString = coreTask.ScheduleString;
            TaskPriority = coreTask.Priority;
            Deleted = coreTask.Deleted;
            UpdatedDate = coreTask.UpdatedDate;
            Project = new ProjectResponseModel(coreTask.Project, null);
            Parent = new TaskResponseModel(coreTask.Parent);
            ReminderSchedule = coreTask.ReminderSchedule;
            Reminder = coreTask.Reminder;
            AssignedBy = new UserDTO(coreTask.AssignedByUser);
            AssignedFor = new UserDTO(coreTask.AssignedForUser);
            CreatedBy = new UserDTO(coreTask.CreatedByUser);
        }
        public long Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? Schedule { get; set; }
        public string ScheduleString { get; set; }
        public PriorityLevel TaskPriority { get; set; }
        public bool Deleted { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ProjectResponseModel Project { get; set; }
        public TaskResponseModel Parent { get; set; }
        public DateTime? ReminderSchedule { get; set; }
        public bool Reminder { get; set; }
        public UserDTO AssignedBy { get; set; }
        public UserDTO AssignedFor { get; set; }
        public UserDTO CreatedBy { get; set; }
    }
}
