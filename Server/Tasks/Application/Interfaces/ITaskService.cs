using Core.Application.Models;

namespace Core.Application.Interfaces
{
    public interface ITaskService
    {
        int AddNewTask(NewTaskModel newTask);
    }
}
