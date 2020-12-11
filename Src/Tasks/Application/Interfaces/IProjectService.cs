using Core.Application.Models;
using Core.Application.Models.Project;
using Core.Domain.DbEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public interface IProjectService
    {
        public Task<ProjectResponseModel> AddNewProject(long createdByUserId, NewProjectModel newProject);
        public Task<IEnumerable<ProjectResponseModel>> GetAllProjects(GetAllProjectsModel model);
        public Task<ProjectResponseModel> GetOneProject(GetOneProjectModel model);
        public Task<ProjectResponseModel> UpdateProjectInfo(long projectId, long updatedByUserId, UpdateProjectInfoModel model);
        public Task<ProjectResponseModel> SoftDeleteExistingProject(long projectId, long deletedByUserId);
    }
}
