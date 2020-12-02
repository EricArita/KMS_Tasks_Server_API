using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Domain.DbEntities;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Services
{
    public class ProjectService : IProjectService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly UserManager<ApplicationUser> _userManager;

        public ProjectService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        async Task<Project> IProjectService.AddNewProject(NewProjectModel newProject)
        {
            if (newProject.Name == null || newProject.Name.Length <= 0) throw new Exception("Cannot create new project without a name");

            Project addedProject = new Project()
            {
                Name = newProject.Name,
                Description = newProject.Description,
                CreatedBy = newProject.CreatedBy,
                UpdatedBy = newProject.CreatedBy,
                ParentId = newProject.ParentId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                Deleted = false,
            };

            _unitOfWork.Repository<Project>().Insert(addedProject);

            _unitOfWork.SaveChanges();

            return addedProject;
        }

        async Task<IEnumerable<Project>> IProjectService.GetAllProjects(GetAllProjectsModel model)
        {
            var result = _unitOfWork.Repository<Project>().Get(filter: e => (e.CreatedBy.HasValue && e.CreatedBy.Value == model.UserID)
                                 || (e.UpdatedBy.HasValue && e.UpdatedBy.Value == model.UserID));
            _unitOfWork.SaveChanges();

            return result;
        }
    }
}
