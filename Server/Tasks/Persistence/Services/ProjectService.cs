using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Domain.DbEntities;
using Infrastructure.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Services
{
    public class ProjectService : IProjectService
    {
        protected IUnitOfWork _unitOfWork;

        public ProjectService(IUnitOfWork unitOfWork, )
        {
            _unitOfWork = (UnitOfWork) unitOfWork;
        }

        async Task<Project> IProjectService.AddNewProject(NewProjectModel newProject)
        {
            if (newProject.Name == null || newProject.Name.Length <= 0) throw new Exception("Cannot create new project without a name");

            var currentUser = _unitOfWork.Repository<AspNetUsers>().Get(filter: user => user.Id == newProject.CreatedBy).GetEnumerator();

            if (currentUser.Current == null) throw new Exception("I cannot find the user you are asking for");
            var toBeInteractedUser = currentUser.Current;
            if(currentUser.MoveNext())  throw new Exception ("Found more than one user with the credentials");

            Project toBeAddedProject = new Project()
            {
                Name = newProject.Name,
                Description = newProject.Description,
                CreatedBy = (int) toBeInteractedUser.UserId,
                UpdatedBy = (int)toBeInteractedUser.UserId,
                ParentId = newProject.ParentId,
                CreatedDate = DateTime.UtcNow,
                UpdatedDate = DateTime.UtcNow,
                Deleted = false,
            };

            _unitOfWork.Repository<Project>().Insert(toBeAddedProject);

            _unitOfWork.SaveChanges();

            return toBeAddedProject;
        }

        async Task<IEnumerable<Project>> IProjectService.GetAllProjects(GetAllProjectsModel model)
        {
            if (model.UserID == null) throw new Exception("Cannot find projects of this user if you don't provide a UserID");

            var currentUser = _unitOfWork.Repository<AspNetUsers>().Get(filter: user => user.Id == model.UserID).GetEnumerator();

            if (currentUser.Current == null) throw new Exception("I cannot find the user you are asking for");
            var toBeInteractedUser = currentUser.Current;
            if (currentUser.MoveNext()) throw new Exception("Found more than one user with the credentials");

            var result = _unitOfWork.Repository<Project>().Get(filter: e => (e.CreatedBy.HasValue && e.CreatedBy.Value == toBeInteractedUser.UserId)
                                 || (e.UpdatedBy.HasValue && e.UpdatedBy.Value == toBeInteractedUser.UserId));
            _unitOfWork.SaveChanges();

            return result;
        }
    }
}
