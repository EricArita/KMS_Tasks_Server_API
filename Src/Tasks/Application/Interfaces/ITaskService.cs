using Core.Application.Models;
using Core.Application.Models.Task;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public interface ITaskService
    {
        public Task<TaskResponseModel> AddNewTask(long createdByUserId, NewTaskModel newTask);
        public Task<IEnumerable<TaskResponseModel>> GetAllTasks(GetAllTasksModel model);
        public Task<TaskResponseModel> GetOneTask(GetOneTaskModel model);
        public Task<TaskResponseModel> UpdateTaskInfo(long taskId, long updatedByUserId, UpdateTaskInfoModel model);
        public Task<TaskResponseModel> SoftDeleteExistingTask(long taskId, long deletedByUserId);
    }
}
