using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Domain.DbEntities;
using Infrastructure.Persistence.Repositories;
using System;

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
                SectionId = task.SectionId,
                ParentId = task.ParentId,
                Reminder = task.Reminder,
                ReminderSchedule = task.ReminderSchedule,
                AssignedBy = task.AssignedBy,
                AssignedFor = task.AssignedFor,
                CreatedBy = task.CreatedBy,
                CreatedDate = DateTime.UtcNow,
            };

            this.unitOfWork.Repository<TaskRepository>().AddNewTask(newTask);
            var res = this.unitOfWork.SaveChanges();

            return res;
        }
    }
}
