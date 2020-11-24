using Core.Domain.DbEntities;
using Infrastructure.Persistence.Contexts;
using System;
using System.Linq;
using System.Collections.Generic;
using WebApi.Models;
using static Core.Domain.Constants.Enums;

namespace Infrastructure.Persistence.Repositories
{
    public class TaskRepository : GenericRepositoryBase<Tasks>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public bool AddNewTask(NewTaskModel task)
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

        public IEnumerable<Tasks> GetAllTasks(int userId, byte category)
        {
            var query =  base.Get(filter: e => (e.CreatedBy.HasValue && e.CreatedBy.Value == userId) || (e.AssignedFor.HasValue && e.AssignedFor.Value == userId)
                                  || (e.AssignedBy.HasValue && e.AssignedBy.Value == userId));
            switch (category)
            {
                case (byte)MenuSidebarOptions.Today:
                    return query.Where(e => e.Schedule.HasValue && e.Schedule.Value == DateTime.Today).ToList();
                case (byte)MenuSidebarOptions.Upcoming:
                    return query.Where(e => e.Schedule.HasValue && e.Schedule.Value > DateTime.Today).ToList();
                default:
                    return query.Where(e => e != null).ToList(); ;
            }
        }
    }
}
