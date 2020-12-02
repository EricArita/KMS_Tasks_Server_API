using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Domain.DbEntities;
using Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using static Core.Domain.Constants.Enums;

namespace Infrastructure.Persistence.Services
{
    public class TaskService : ITaskService
    {
        private IUnitOfWork unitOfWork;

        public TaskService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public int AddNewTask(NewTaskModel task)
        {
            var newTask = new Tasks()
            {
                Name = task.Name,
                Schedule = task.Schedule,
                ScheduleString = task.ScheduleString,
                PriorityId = task.PriorityId,
                ProjectId = task.ProjectId,
                ParentId = task.ParentId,
                Reminder = task.Reminder,
                ReminderSchedule = task.ReminderSchedule,
                AssignedBy = task.AssignedBy,
                AssignedFor = task.AssignedFor,
                CreatedBy = task.CreatedBy,
                CreatedDate = DateTime.UtcNow,
            };

            this.unitOfWork.Repository<Tasks>().Insert(newTask);
            var res = this.unitOfWork.SaveChanges();

            return res;
        }

        public IEnumerable<Tasks> GetAllTasks(int userId, byte category)
        {
            var query = this.unitOfWork.Repository<Tasks>().Get(filter: e => (e.CreatedBy.HasValue && e.CreatedBy.Value == userId) || (e.AssignedFor.HasValue && e.AssignedFor.Value == userId)
                                 || (e.AssignedBy.HasValue && e.AssignedBy.Value == userId));
            switch (category)
            {
                case (byte)MenuSidebarOptions.Today:
                    return query.Where(e => e.Schedule.HasValue && e.Schedule.Value == DateTime.Today).ToList();
                case (byte)MenuSidebarOptions.Upcoming:
                    return query.Where(e => e.Schedule.HasValue && e.Schedule.Value > DateTime.Today).ToList();
                default:
                    return query.Where(e => e != null).ToList();
            }
        }
    }
}
