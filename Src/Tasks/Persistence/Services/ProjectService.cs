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
using Core.Application.Models.Project;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Persistence.Services
{
    public class ProjectService : IProjectService
    {
        protected readonly IUnitOfWork _unitOfWork;
        private ILogger<ProjectService> _logger;
        protected readonly UserManager<ApplicationUser> _userManager;

        public ProjectService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ILogger<ProjectService> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }

        async Task<ProjectResponseModel> IProjectService.AddNewProject(NewProjectModel newProject)
        {
            if (!newProject.CreatedBy.HasValue) throw new ProjectServiceException(400, "Cannot create new project with no user relation");

            if (newProject.Name == null || newProject.Name.Length <= 0) throw new ProjectServiceException(400, "Cannot create new project without a name");

            await using var t = await _unitOfWork.CreateTransaction();

            try
            {
                // Check if uid is valid or not
                ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == (newProject.CreatedBy ?? null));
                if (validUser == null)
                {
                    throw new ProjectServiceException(404, "Cannot locate a valid user from the claim provided");
                }

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

                return new ProjectResponseModel(addedProject, relationToUser.ProjectRole);
            }
            catch (Exception ex)
            {
                await t.RollbackAsync();
                _logger.LogError(ex, "An error occurred when using ProjectService");
                throw ex;
            }
        }

        async Task<IEnumerable<ProjectResponseModel>> IProjectService.GetAllProjects(GetAllProjectsModel model)
        {
            if (model.UserID == null) throw new ProjectServiceException(400, "Cannot find projects of this user if you don't provide a UserID");

            await using var t = await _unitOfWork.CreateTransaction();

            try
            {
                // Check if uid is valid or not
                ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == model.UserID);
                if (validUser == null)
                {
                    throw new ProjectServiceException(404, "Cannot locate a valid user from the claim provided");
                }

                // Query for  all the projects user participated in
                var result = from userProjects in _unitOfWork.Repository<UserProjects>().GetDbset()
                             join relatedProjects in _unitOfWork.Repository<Project>().GetDbset() on userProjects.ProjectId equals relatedProjects.Id
                             join projectRoles in _unitOfWork.Repository<ProjectRole>().GetDbset() on userProjects.RoleId equals projectRoles.Id
                             where userProjects.UserId == model.UserID
                             select new ProjectResponseModel(relatedProjects, projectRoles);

                await _unitOfWork.SaveChangesAsync();

                return result.ToList();
            }
            catch (Exception ex)
            {
                await t.RollbackAsync();
                _logger.LogError(ex, "An error occurred when using ProjectService");
                throw ex;
            }
}

        async Task<ProjectResponseModel> IProjectService.GetOneProject(GetOneProjectModel model)
        {
            if (model.UserId == null || model.ProjectId == null) throw new ProjectServiceException(400, "Cannot find projects of this user if you don't provide a UserID");

            await using var t = await _unitOfWork.CreateTransaction();

            try
            {
                // Check if uid is valid or not
                ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == model.UserId);
                if (validUser == null)
                {
                    throw new ProjectServiceException(404, "Cannot locate a valid user from the claim provided");
                }

                // Query for participations in projects with the provided info
                var result = from userProjects in _unitOfWork.Repository<UserProjects>().GetDbset()
                             join relatedProjects in _unitOfWork.Repository<Project>().GetDbset() on userProjects.ProjectId equals relatedProjects.Id
                             join projectRoles in _unitOfWork.Repository<ProjectRole>().GetDbset() on userProjects.RoleId equals projectRoles.Id
                             where userProjects.UserId == model.UserId && userProjects.ProjectId == model.ProjectId
                             select new ProjectResponseModel(relatedProjects, projectRoles);

                // If cannot find the project from the infos provided, return a service exception
                if(result.Count() < 1)
                {
                    throw new ProjectServiceException(404, "Cannot find a single instance of a project from the infos you provided");
                }

                // If found more than one instance, the database probably is corrupted, in this case, return an internal error
                if(result.Count() > 1)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Inconsistency in database. Executing query returns more than one result: ");
                    sb.AppendLine(result.ToList().ToString());
                    throw new Exception(sb.ToString());
                }

                await _unitOfWork.SaveChangesAsync();

                return result.ToList()[0];
            }
            catch (Exception ex)
            {
                await t.RollbackAsync();
                _logger.LogError(ex, "An error occurred when using ProjectService");
                throw ex;
            }
        }

        public async Task<ProjectResponseModel> UpdateProjectInfo(int projectId, UpdateProjectInfoModel model)
        {
            if (!model.CreatedBy.HasValue) throw new ProjectServiceException(400, "Cannot create new project with no user relation");

            // Start the update transaction
            await using var t = await _unitOfWork.CreateTransaction();
            try
            {
                // Check if uid is valid or not
                ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == (model.CreatedBy ?? null));
                if (validUser == null)
                {
                    throw new ProjectServiceException(404, "Cannot locate a valid user from the claim provided");
                }

                // Check if project is in db first
                var result = from project in _unitOfWork.Repository<Project>().GetDbset()
                             where project.Id == projectId
                             select project;
                if (result == null || result.Count() < 1)
                {
                    throw new ProjectServiceException(404, "Cannot find a single instance of a project from the infos you provided");
                }
                if (result.Count() > 1)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Inconsistency in database. Executing query returns more than one result: ");
                    sb.AppendLine(result.ToList().ToString());
                    throw new Exception(sb.ToString());
                }

                Project operatedProject = result.ToList()[0];

                // flag to know if any field is going to be changed or not
                bool isUpdated = false;

                // Check if its parent project is valid
                if (model.ParentId != null)
                {
                    var parent = from project in _unitOfWork.Repository<Project>().GetDbset()
                                 where project.Id == model.ParentId
                                 select project;
                    if (parent == null || parent.Count() < 1)
                    {
                        throw new ProjectServiceException(404, "Cannot find a single instance of a project from the infos you provided");
                    }
                    if (parent.Count() > 1)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("Inconsistency in database. Executing query returns more than one result: ");
                        sb.AppendLine(parent.ToList().ToString());
                        throw new Exception(sb.ToString());
                    }

                    Project newParentProject = parent.ToList()[0];

                    if(newParentProject.Id == operatedProject.Id)
                    {
                        throw new ProjectServiceException(400, "Cannot set a project to be its own parent");
                    }

                    // Only  register change only if parentId is not sent together with removefromparent field (we ignore the change)
                    if (newParentProject.Id != operatedProject.ParentId && (model.MakeParentless == null || !model.MakeParentless.Value))
                    {
                        operatedProject.ParentId = newParentProject.Id;
                        isUpdated = true;
                    }
                }

                // If remove parent is true and the item has a parent, then we register the change
                if (operatedProject.ParentId != null && model.MakeParentless != null && model.MakeParentless.Value)
                {
                    operatedProject.ParentId = null;
                    isUpdated = true;
                }

                //Update fields
                if (model.Name != null && model.Name.Length > 0 && model.Name != operatedProject.Name)
                {
                    operatedProject.Name = model.Name;
                    isUpdated = true;
                }
                if (model.Description != null && model.Description.Length > 0 && model.Description != operatedProject.Description)
                {
                    operatedProject.Description = model.Description;
                    isUpdated = true;
                }

                // Get project role for the response
                var getUserProject = from userProject in _unitOfWork.Repository<UserProjects>().GetDbset()
                                     where userProject.UserId == validUser.UserId && userProject.ProjectId == operatedProject.Id
                                     select userProject;
                if (getUserProject == null || getUserProject.Count() < 1)
                {
                    throw new ProjectServiceException(404, "Cannot find a single instance of a project from the infos you provided");
                }
                if (getUserProject.Count() > 1)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Inconsistency in database. Executing query returns more than one result: ");
                    sb.AppendLine(getUserProject.ToList().ToString());
                    throw new Exception(sb.ToString());
                }

                // If there is any update, we update the object
                if (isUpdated) {
                    operatedProject.UpdatedBy = validUser.UserId;
                    operatedProject.UpdatedDate = DateTime.UtcNow;
                    _unitOfWork.Repository<Project>().Update(operatedProject);
                    await _unitOfWork.SaveChangesAsync();
                }       

                await t.CommitAsync();

                return new ProjectResponseModel(operatedProject, getUserProject.ToList()[0].ProjectRole);
            }
            catch (Exception ex)
            {
                await t.RollbackAsync();
                _logger.LogError(ex, "An error occurred when using ProjectService");
                throw ex;
            }
        }

        public async Task<ProjectResponseModel> SoftDeleteExistingProject(int projectId, int deletedByUserId)
        {
            // Start the update transaction
            await using var t = await _unitOfWork.CreateTransaction();
            try
            {
                // Check if uid is valid or not
                ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == deletedByUserId);
                if (validUser == null)
                {
                    throw new ProjectServiceException(404, "Cannot locate a valid user from the claim provided");
                }

                // Check if project is in db first
                var result = from project in _unitOfWork.Repository<Project>().GetDbset()
                             where project.Id == projectId
                             select project;
                if (result == null || result.Count() < 1)
                {
                    throw new ProjectServiceException(404, "Cannot find a single instance of a project from the infos you provided");
                }
                if (result.Count() > 1)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Inconsistency in database. Executing query returns more than one result: ");
                    sb.AppendLine(result.ToList().ToString());
                    throw new Exception(sb.ToString());
                }

                Project operatedProject = result.ToList()[0];

                // flag to know if any field is going to be changed or not
                bool isUpdated = false;

                if (!operatedProject.Deleted)
                {
                    operatedProject.Deleted = true;
                    isUpdated = true;
                }

                // Get project role for the response
                var getUserProject = from userProject in _unitOfWork.Repository<UserProjects>().GetDbset()
                                     where userProject.UserId == validUser.UserId && userProject.ProjectId == operatedProject.Id
                                     select userProject;
                if (getUserProject == null || getUserProject.Count() < 1)
                {
                    throw new ProjectServiceException(404, "Cannot find a single instance of a project from the infos you provided");
                }
                if (getUserProject.Count() > 1)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("Inconsistency in database. Executing query returns more than one result: ");
                    sb.AppendLine(getUserProject.ToList().ToString());
                    throw new Exception(sb.ToString());
                }

                // If there is any update, we update the object
                if (isUpdated)
                {
                    operatedProject.UpdatedBy = validUser.UserId;
                    operatedProject.UpdatedDate = DateTime.UtcNow;
                    _unitOfWork.Repository<Project>().Update(operatedProject);
                    await _unitOfWork.SaveChangesAsync();
                }

                await t.CommitAsync();

                return new ProjectResponseModel(operatedProject, getUserProject.ToList()[0].ProjectRole);
            }
            catch (Exception ex)
            {
                await t.RollbackAsync();
                _logger.LogError(ex, "An error occurred when using ProjectService");
                throw ex;
            }
        }
    }
}
