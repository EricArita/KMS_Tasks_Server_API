using Core.Application.Helper.Exceptions.Project;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Domain.Constants;
using Core.Domain.DbEntities;
using Infrastructure.Persistence.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Infrastructure.Persistence.DTOs;

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
            if (!newProject.CreatedBy.HasValue) throw new ProjectServiceException("Cannot create new project with no user relation");

            if (newProject.Name == null || newProject.Name.Length <= 0) throw new ProjectServiceException("Cannot create new project without a name");

            await using var t = await _unitOfWork.CreateTransaction();

            try
            {
                // Add project in first
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
                addedProject = await _unitOfWork.Repository<Project>().InsertAsync(addedProject);
                await _unitOfWork.SaveChangesAsync();

                // Add user project: project's relation to an owner
                UserProjects relationToUser = new UserProjects()
                {
                    UserId = newProject.CreatedBy.Value,
                    ProjectId = addedProject.Id,
                    RoleId = Enums.ProjectRoles.Owner,
                };
                await _unitOfWork.Repository<UserProjects>().InsertAsync(relationToUser);
                await _unitOfWork.SaveChangesAsync();

                await t.CommitAsync();

                return addedProject;
            }
            catch (Exception ex)
            {
                await t.RollbackAsync();
                throw ex;
            }
        }

        async Task<IEnumerable<object>> IProjectService.GetAllProjects(GetAllProjectsModel model)
        {
            if (model.UserID == null) throw new ProjectServiceException("Cannot find projects of this user if you don't provide a UserID");

            // Query for  all the projects user participated in
            var result = from userProjects in _unitOfWork.Repository<UserProjects>().GetDbset()
                         join relatedProjects in _unitOfWork.Repository<Project>().GetDbset() on userProjects.ProjectId equals relatedProjects.Id
                         join projectRoles in _unitOfWork.Repository<ProjectRole>().GetDbset() on userProjects.RoleId equals projectRoles.Id
                         where userProjects.UserId == model.UserID
                         select new { 
                             Project = new
                             {
                                 relatedProjects.Id,
                                 relatedProjects.Name,
                                 relatedProjects.Description,
                                 relatedProjects.CreatedDate,
                                 CreatedBy = new UserDTO(relatedProjects.CreatedByUser),
                                 relatedProjects.UpdatedDate,
                                 UpdatedBy = new UserDTO(relatedProjects.UpdatedByUser),
                                 Parent = relatedProjects.Parent != null ? new
                                 {
                                     relatedProjects.Parent.Id,
                                     relatedProjects.Parent.Name,
                                     relatedProjects.Parent.Description,
                                     relatedProjects.Parent.CreatedDate,
                                     CreatedBy = new UserDTO(relatedProjects.Parent.CreatedByUser),
                                     relatedProjects.Parent.UpdatedDate,
                                     UpdatedBy = new UserDTO(relatedProjects.Parent.UpdatedByUser)
                                 } : null,
                                 ProjectRole = projectRoles
                             }
                         };

            await _unitOfWork.SaveChangesAsync();

            return result.ToList();
        }

        async Task<object> IProjectService.GetOneProject(GetOneProjectModel model)
        {
            if (model.UserId == null || model.ProjectId == null) throw new ProjectServiceException("Cannot find projects of this user if you don't provide a UserID");

            // Query for participations in projects with the provided info
            var result = from userProjects in _unitOfWork.Repository<UserProjects>().GetDbset()
                         join relatedProjects in _unitOfWork.Repository<Project>().GetDbset() on userProjects.ProjectId equals relatedProjects.Id
                         join projectRoles in _unitOfWork.Repository<ProjectRole>().GetDbset() on userProjects.RoleId equals projectRoles.Id
                         where userProjects.UserId == model.UserId && userProjects.ProjectId == model.ProjectId
                         select new
                         {
                             Project = new
                             {
                                 relatedProjects.Id,
                                 relatedProjects.Name,
                                 relatedProjects.Description,
                                 relatedProjects.CreatedDate,
                                 CreatedBy = new UserDTO(relatedProjects.CreatedByUser),
                                 relatedProjects.UpdatedDate,
                                 UpdatedBy = new UserDTO(relatedProjects.UpdatedByUser),
                                 Parent = relatedProjects.Parent != null ? new
                                 {
                                     relatedProjects.Parent.Id,
                                     relatedProjects.Parent.Name,
                                     relatedProjects.Parent.Description,
                                     relatedProjects.Parent.CreatedDate,
                                     CreatedBy = new UserDTO(relatedProjects.Parent.CreatedByUser),
                                     relatedProjects.Parent.UpdatedDate,
                                     UpdatedBy = new UserDTO(relatedProjects.Parent.UpdatedByUser)
                                 } : null,
                                 ProjectRole = projectRoles
                             }
                         };

            // If cannot find the project from the infos provided, return a service exception
            if(result.Count<object>() < 1)
            {
                throw new ProjectServiceException("Cannot find a single instance of a project from the infos you provided");
            }

            // If found more than one instance, the database probably is corrupted, in this case, return an internal error
            if(result.Count<object>() > 1)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Inconsistency in database. Executing query returns more than one result: ");
                sb.AppendLine(result.ToList().ToString());
                throw new Exception();
            }

            await _unitOfWork.SaveChangesAsync();

            return result.ToList()[0];
        }
    }
}
