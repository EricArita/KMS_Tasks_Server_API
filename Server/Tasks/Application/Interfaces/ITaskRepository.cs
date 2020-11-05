using Core.Application.Interfaces;
using Core.Domain.DbEntities;
using WebApi.Models;

namespace Infrastructure.Persistence.Repositories
{
    public interface ITaskRepository : IGenericRepositoryBase<Tasks>
    {
        bool AddNewTask(TaskRequestModel task);
    }
}