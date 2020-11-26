using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Domain.DbEntities;
using System.Collections.Generic;

namespace Infrastructure.Persistence.Repositories
{
    public interface ITaskRepository : IRepositoryBase<Tasks>
    {
        bool AddNewTask(Tasks task);
        IEnumerable<Tasks> GetAllTasks(int userId, byte category);
    }
}