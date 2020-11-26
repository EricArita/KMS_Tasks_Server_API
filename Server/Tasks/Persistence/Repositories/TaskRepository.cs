using Core.Domain.DbEntities;
using Infrastructure.Persistence.Contexts;
using System;
using System.Linq;
using System.Collections.Generic;
using static Core.Domain.Constants.Enums;
using Core.Application.Models;

namespace Infrastructure.Persistence.Repositories
{
    public class TaskRepository : GenericRepositoryBase<Tasks>, ITaskRepository
    {
        public TaskRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public bool AddNewTask(Tasks newTask)
        {
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
