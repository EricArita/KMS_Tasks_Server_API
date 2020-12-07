using Core.Application.Models;
using Core.Domain.DbEntities;
using System.Collections.Generic;

namespace Core.Application.Interfaces
{
    public interface ITaskService
    {
        int AddNewTask(NewTaskModel newTask);
        public IEnumerable<Tasks> GetAllTasks(int userId, byte category);
    }
}
