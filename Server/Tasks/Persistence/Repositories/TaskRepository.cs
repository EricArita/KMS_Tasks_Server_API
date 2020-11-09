using Core.Domain.DbEntities;
using Infrastructure.Persistence.Contexts;
using System;
using WebApi.Models;

namespace Infrastructure.Persistence.Repositories
{
    public class TaskRepository : GenericRepositoryBase<Tasks>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public bool AddNewTask(TaskRequestModel task)
        {
            var newTask = new Tasks()
            {
                Name = task.Name,
                Schedule = task.Schedule,
                ScheduleString = task.ScheduleString,
                PriorityId = task.PriorityId,
                ProjectId = task.ProjectId,
                SectionId = task.SectionId,
                ParentId = task.ParentId,
                Reminder = task.Reminder,
                ReminderSchedule = task.ReminderSchedule,
                AssignedBy = task.AssignedBy,
                AssignedFor = task.AssignedFor,
                CreatedBy = task.CreatedBy,
                CreatedDate = DateTime.UtcNow,
            };
            return base.Insert(newTask);
        }
    }
}
