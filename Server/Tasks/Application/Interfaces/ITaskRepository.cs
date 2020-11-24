using Core.Application.Interfaces;
using Core.Domain.DbEntities;
using System.Collections.Generic;
using WebApi.Models;

namespace Infrastructure.Persistence.Repositories
{
    public interface ITaskRepository : IRepositoryBase<Tasks>
    {
        bool AddNewTask(NewTaskModel task);
        IEnumerable<Tasks> GetAllTasks(int userId, byte category);
    }
}