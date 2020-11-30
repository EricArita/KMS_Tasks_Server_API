using Core.Application.Models;
using Core.Domain.DbEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public interface IProjectService
    {
        public Task<Project> AddNewProject(NewProjectModel newProject);
        public Task<IEnumerable<Project>> GetAllProjects(GetAllProjectsModel model);
    }
}
