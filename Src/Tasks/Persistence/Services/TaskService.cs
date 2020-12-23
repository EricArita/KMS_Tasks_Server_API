using Core.Application.Helper.Exceptions.Task;
using Core.Application.Interfaces;
using Core.Application.Models;
using Core.Application.Models.Task;
using Core.Domain.DbEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Core.Domain.Constants.Enums;
using Core.Domain.Constants;

namespace Infrastructure.Persistence.Services
{
    public class TaskService : ITaskService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected ILogger<TaskService> _logger;
        protected readonly UserManager<ApplicationUser> _userManager;

        public TaskService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ILogger<TaskService> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<TaskResponseModel> AddNewTask(long createdByUserId, NewTaskModel task)
        {
            await using var transaction = await _unitOfWork.CreateTransaction();

            try
            {
                // Check if uid is valid or not
                ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == createdByUserId);
                if (validUser == null)
                {
                    throw new TaskServiceException(UserRelatedErrorsConstants.USER_NOT_FOUND);
                }

                // Check if its associated project is valid
                var parentProject = from project in _unitOfWork.Repository<Project>().GetDbset()
                                 where project.Id == task.ProjectId
                                 select project;
                if (parentProject == null || parentProject.Count() < 1)
                {
                    throw new TaskServiceException(ProjectRelatedErrorsConstants.PARENT_PROJECT_NOT_FOUND);
                }
                if (parentProject.Count() > 1)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(InternalServerErrorsConstants.DATABASE_INTEGRITY_NOT_MAINTAINED);
                    sb.AppendLine(parentProject.ToList().ToString());
                    throw new Exception(sb.ToString());
                }

                // Check if its parent task is valid
                if (task.ParentId != null)
                {
                    var parentTask = from Task in _unitOfWork.Repository<Tasks>().GetDbset()
                                 where Task.Id == task.ParentId
                                 select Task;
                    if (parentTask == null || parentTask.Count() < 1)
                    {
                        throw new TaskServiceException(TaskRelatedErrorsConstants.PARENT_TASK_NOT_FOUND);
                    }
                    if (parentTask.Count() > 1)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(InternalServerErrorsConstants.DATABASE_INTEGRITY_NOT_MAINTAINED);
                        sb.AppendLine(parentTask.ToList().ToString());
                        throw new Exception(sb.ToString());
                    }

                    // If parent task is not in the same project =>  it's false
                    if(parentTask.ToList()[0].ProjectId != parentProject.ToList()[0].Id)
                    {
                        throw new TaskServiceException(TaskRelatedErrorsConstants.PARENT_TASK_OF_A_TASK_ISFROM_ANOTHER_PROJECT);
                    }
                }

                // Check if its assignedBy person is in the Db
                if (task.AssignedBy != null)
                {
                    ApplicationUser assignedByUser = _userManager.Users.FirstOrDefault(e => e.UserId == task.AssignedBy);
                    if (assignedByUser == null)
                    {
                        throw new TaskServiceException(TaskRelatedErrorsConstants.ASSIGNED_BY_FIELD_INVALID);
                    }
                }

                // Check if its assignedFor person is in the Db
                if (task.AssignedFor != null)
                {
                    ApplicationUser assignedForUser = _userManager.Users.FirstOrDefault(e => e.UserId == task.AssignedFor);
                    if (assignedForUser == null)
                    {
                        throw new TaskServiceException(TaskRelatedErrorsConstants.ASSIGNED_FOR_FIELD_INVALID);
                    }
                }

                DateTime insertTime = DateTime.UtcNow;
                // Add task in first
                Tasks newTask = new Tasks()
                {
                    Name = task.Name,
                    Schedule = task.Schedule,
                    ScheduleString = task.ScheduleString,
                    PriorityId = task.PriorityId == 0 ? null : task.PriorityId,
                    ProjectId = task.ProjectId,
                    ParentId = task.ParentId,
                    Reminder = task.Reminder,
                    ReminderSchedule = task.ReminderSchedule,
                    AssignedBy = task.AssignedBy,
                    AssignedFor = task.AssignedFor,
                    CreatedBy = validUser.UserId,
                    CreatedDate = insertTime,
                    UpdatedBy = validUser.UserId,
                    UpdatedDate = insertTime,
                    Deleted = false,
                };  
                await _unitOfWork.Repository<Tasks>().InsertAsync(newTask);
                await _unitOfWork.SaveChangesAsync();

                // Eager load instance for initialization of response model
                var entry = _unitOfWork.Entry(newTask);
                await entry.Reference(e => e.Project).LoadAsync();
                await entry.Reference(e => e.Priority).LoadAsync();
                await entry.Reference(e => e.Parent).LoadAsync();

                await transaction.CommitAsync();

                return new TaskResponseModel(newTask);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, ErrorLoggingMessagesConstants.TASK_SERVICE_ERROR_LOG_MESSAGE);
                throw ex;
            }
        }

        public async Task<IEnumerable<TaskResponseModel>> GetAllTasks(GetAllTasksModel model)
        {
            await using var transaction = await _unitOfWork.CreateTransaction();

            try
            {
                // Check if uid is valid or not
                ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == model.UserId);
                if (validUser == null)
                {
                    throw new TaskServiceException(UserRelatedErrorsConstants.USER_NOT_FOUND);
                }

                // Query for all the tasks in projects that the user participated in and that matches the queries
                var result = from userProjects in _unitOfWork.Repository<UserProjects>().GetDbset()
                             join tasks in _unitOfWork.Repository<Tasks>().GetDbset() on userProjects.ProjectId equals tasks.ProjectId
                             where userProjects.UserId == validUser.UserId && tasks.Deleted == false
                             select tasks;

                // Run original statement through additional queries
                if(model.ProjectId != null)
                {
                    result.Where(e => e.ProjectId == model.ProjectId);
                }

                if(model.CategoryType != null)
                {
                    switch (model.CategoryType)
                    {
                        case (byte)MenuSidebarOptions.Today:
                            result.Where(e => e.Schedule.HasValue && e.Schedule.Value == DateTime.Today); break;
                        case (byte)MenuSidebarOptions.Upcoming:
                            result.Where(e => e.Schedule.HasValue && e.Schedule.Value > DateTime.Today); break;
                        default:
                            result.Where(e => e != null); break;
                    }
                }

                // Finally, convert the result by selecting in the form of response (eager load everything)
                var finalResult = result.Include(e => e.Project)
                    .Include(e => e.Priority)
                    .Include(e => e.Parent)
                    .Select(e => new TaskResponseModel(e));

                await _unitOfWork.SaveChangesAsync();

                await transaction.CommitAsync();

                return finalResult.ToList();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, ErrorLoggingMessagesConstants.TASK_SERVICE_ERROR_LOG_MESSAGE);
                throw ex;
            }
        }

        public async Task<TaskResponseModel> GetOneTask(GetOneTaskModel model)
        {
            await using var transaction = await _unitOfWork.CreateTransaction();

            try
            {
                // Check if uid is valid or not
                ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == model.UserId);
                if (validUser == null)
                {
                    throw new TaskServiceException(UserRelatedErrorsConstants.USER_NOT_FOUND);
                }

                // Query for all the tasks in projects that the user participated in and that matches the queries
                var result = from userProjects in _unitOfWork.Repository<UserProjects>().GetDbset()
                             join tasks in _unitOfWork.Repository<Tasks>().GetDbset() on userProjects.ProjectId equals tasks.ProjectId
                             where userProjects.UserId == validUser.UserId && tasks.Deleted == false && tasks.Id == model.TaskId
                             select tasks;

                // If cannot find the project from the infos provided, return a service exception
                if (result == null || result.Count() < 1)
                {
                    throw new TaskServiceException(TaskRelatedErrorsConstants.TASK_NOT_FOUND);
                }
                // Corrupted Db
                if (result.Count() > 1)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(InternalServerErrorsConstants.DATABASE_INTEGRITY_NOT_MAINTAINED);
                    sb.AppendLine(result.ToList().ToString());
                    throw new Exception(sb.ToString());
                }

                // Finally, convert the result by selecting in the form of response (eager load everything)
                var finalResult = result.Include(e => e.Project)
                    .Include(e => e.Priority)
                    .Include(e => e.Parent)
                    .Select(e => new TaskResponseModel(e));

                await _unitOfWork.SaveChangesAsync();

                await transaction.CommitAsync();

                return finalResult.ToList()[0];
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, ErrorLoggingMessagesConstants.TASK_SERVICE_ERROR_LOG_MESSAGE);
                throw ex;
            }
        }

        public async Task<TaskResponseModel> UpdateTaskInfo(long taskId, long updatedByUserId, UpdateTaskInfoModel model)
        {
            // Start the update transaction
            await using var transaction = await _unitOfWork.CreateTransaction();
            try
            {
                DateTime rightNow = DateTime.UtcNow;

                // Check if uid is valid or not
                ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == updatedByUserId);
                if (validUser == null)
                {
                    throw new TaskServiceException(UserRelatedErrorsConstants.USER_NOT_FOUND);
                }

                // Check if task is in db first
                var result = from task in _unitOfWork.Repository<Tasks>().GetDbset()
                             where task.Id == taskId
                             select task;
                if (result == null || result.Count() < 1)
                {
                    throw new TaskServiceException(TaskRelatedErrorsConstants.TASK_NOT_FOUND);
                }
                if (result.Count() > 1)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(InternalServerErrorsConstants.DATABASE_INTEGRITY_NOT_MAINTAINED);
                    sb.AppendLine(result.ToList().ToString());
                    throw new Exception(sb.ToString());
                }

                Tasks operatedTask = result.ToList()[0];

                // Query if the current user is associated with the project that the operated task is in???
                var userIsAssociated = from userProjects in _unitOfWork.Repository<UserProjects>().GetDbset()
                             where userProjects.UserId == validUser.UserId && operatedTask.ProjectId == userProjects.ProjectId
                             select userProjects;
                if (userIsAssociated == null || userIsAssociated.Count() < 1)
                {
                    throw new TaskServiceException(TaskRelatedErrorsConstants.ACCESS_TO_TASK_IS_FORBIDDEN);
                }

                // flag to know if any field is going to be changed or not
                bool isUpdated = false;

                // Check if its new project is valid (if have)
                if (model.ProjectId != null)
                {
                    if (model.ProjectId == operatedTask.ProjectId)
                    {
                        throw new TaskServiceException(TaskRelatedErrorsConstants.CANNOT_SET_NEW_PARENT_PROJECT_TOBE_THE_OLD_VALUE);
                    }

                    var parentProject = from project in _unitOfWork.Repository<Project>().GetDbset()
                                 where project.Id == model.ProjectId
                                 select project;
                    if (parentProject == null || parentProject.Count() < 1)
                    {
                        throw new TaskServiceException(ProjectRelatedErrorsConstants.PARENT_PROJECT_NOT_FOUND);
                    }
                    if (parentProject.Count() > 1)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(InternalServerErrorsConstants.DATABASE_INTEGRITY_NOT_MAINTAINED);
                        sb.AppendLine(parentProject.ToList().ToString());
                        throw new Exception(sb.ToString());
                    }

                    Project newParentProject = parentProject.ToList()[0];

                    //First change the project Id of the task and make it parentless
                    operatedTask.ProjectId = newParentProject.Id;
                    operatedTask.ParentId = null;

                    // Define recursive call to update children tasks to the the new project
                    var tasksDbSet = _unitOfWork.Repository<Tasks>().GetDbset();
                    async Task<bool> recursiveChangeProjectId(Tasks task)
                    {
                        if (task == null) return false;
                        // Find all tasks that have this task as parent
                        var query = tasksDbSet.Where(t => t.ParentId == task.Id);
                        if (query == null || query.Count() < 1) return true;
                        // Stop query and get results
                        var childrenTasks = query.ToList();
                        // For each of them change the project they belong to, to this new parent project
                        foreach (Tasks t in childrenTasks)
                        {
                            t.ProjectId = newParentProject.Id;
                            t.UpdatedBy = validUser.UserId;
                            t.UpdatedDate = rightNow;
                            _unitOfWork.Repository<Tasks>().Update(t);
                            await recursiveChangeProjectId(t);
                        }
                        return true;
                    }

                    // Run the recursive call
                    await recursiveChangeProjectId(operatedTask);     
                    isUpdated = true;
                }

                // Check if its new parent task is valid (if have)
                if (model.ParentId != null)
                {
                    var parentTask = from task in _unitOfWork.Repository<Tasks>().GetDbset()
                                 where task.Id == model.ParentId
                                 select task;
                    if (parentTask == null || parentTask.Count() < 1)
                    {
                        throw new TaskServiceException(TaskRelatedErrorsConstants.PARENT_TASK_NOT_FOUND);
                    }
                    if (parentTask.Count() > 1)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine(InternalServerErrorsConstants.DATABASE_INTEGRITY_NOT_MAINTAINED);
                        sb.AppendLine(parentTask.ToList().ToString());
                        throw new Exception(sb.ToString());
                    }

                    Tasks newParentTask = parentTask.ToList()[0];

                    if (newParentTask.Id == operatedTask.Id)
                    {
                        throw new TaskServiceException(TaskRelatedErrorsConstants.CANNOT_SET_PARENT_TASK_TOBE_ITSELF);
                    }

                    if (newParentTask.ProjectId != operatedTask.ProjectId)
                    {
                        throw new TaskServiceException(TaskRelatedErrorsConstants.PARENT_TASK_OF_A_TASK_ISFROM_ANOTHER_PROJECT);
                    }

                    // Only  register change only if parentId is not sent together with removefromparent field (we ignore the change)
                    if (newParentTask.Id != operatedTask.ParentId && (model.MakeParentless == null || !model.MakeParentless.Value))
                    {
                        operatedTask.ParentId = newParentTask.Id;
                        isUpdated = true;
                    }
                }

                // If remove parent is true and the task item has a parent, then we register the change (if request have MakeParentless)
                if (operatedTask.ParentId != null && model.MakeParentless != null && model.MakeParentless.Value)
                {
                    operatedTask.ParentId = null;
                    isUpdated = true;
                }

                // Check if its assignedBy person is in the Db (if have)
                if (model.AssignedBy != null)
                {
                    ApplicationUser assignedByUser = _userManager.Users.FirstOrDefault(e => e.UserId == model.AssignedBy);
                    if (assignedByUser == null)
                    {
                        throw new TaskServiceException(TaskRelatedErrorsConstants.ASSIGNED_BY_FIELD_INVALID);
                    }
                    else
                    {
                        operatedTask.AssignedBy = assignedByUser.UserId;
                        isUpdated = true;
                    }
                }

                // Check if its assignedFor person is in the Db
                if (model.AssignedFor != null)
                {
                    ApplicationUser assignedForUser = _userManager.Users.FirstOrDefault(e => e.UserId == model.AssignedFor);
                    if (assignedForUser == null)
                    {
                        throw new TaskServiceException(TaskRelatedErrorsConstants.ASSIGNED_FOR_FIELD_INVALID);
                    }
                    else
                    {
                        operatedTask.AssignedFor = assignedForUser.UserId;
                        isUpdated = true;
                    }
                }

                // Priority level
                if(model.PriorityId != null)
                {
                    operatedTask.PriorityId = model.PriorityId == 0 ? null : model.PriorityId;
                    isUpdated = true;
                }

                //Update normal fields that are not references
                if (model.Name != null && model.Name.Length > 0 && model.Name != operatedTask.Name)
                {
                    operatedTask.Name = model.Name;
                    isUpdated = true;
                }

                // If there is any update, we update the object
                if (isUpdated)
                {
                    operatedTask.UpdatedBy = validUser.UserId;
                    operatedTask.UpdatedDate = rightNow;
                    _unitOfWork.Repository<Tasks>().Update(operatedTask);
                    await _unitOfWork.SaveChangesAsync();
                }

                // Eager load instance for initialization of response model
                var entry = _unitOfWork.Entry(operatedTask);
                await entry.Reference(e => e.Project).LoadAsync();
                await entry.Reference(e => e.Priority).LoadAsync();
                await entry.Reference(e => e.Parent).LoadAsync();

                await transaction.CommitAsync();

                return new TaskResponseModel(operatedTask);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, ErrorLoggingMessagesConstants.TASK_SERVICE_ERROR_LOG_MESSAGE);
                throw ex;
            }
        }

        public async Task<TaskResponseModel> SoftDeleteExistingTask(long taskId, long deletedByUserId)
        {
            // Start the update transaction
            await using var transaction = await _unitOfWork.CreateTransaction();
            try
            {
                DateTime rightNow = DateTime.UtcNow;

                // Check if uid is valid or not
                ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == deletedByUserId);
                if (validUser == null)
                {
                    throw new TaskServiceException(UserRelatedErrorsConstants.USER_NOT_FOUND);
                }

                // Check if task is in db first
                var result = from task in _unitOfWork.Repository<Tasks>().GetDbset()
                             where task.Id == taskId
                             select task;
                if (result == null || result.Count() < 1)
                {
                    throw new TaskServiceException(TaskRelatedErrorsConstants.TASK_NOT_FOUND);
                }
                if (result.Count() > 1)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine(InternalServerErrorsConstants.DATABASE_INTEGRITY_NOT_MAINTAINED);
                    sb.AppendLine(result.ToList().ToString());
                    throw new Exception(sb.ToString());
                }

                Tasks operatedTask = result.ToList()[0];

                // Get if user have the authorization to change tag info (query the projects the user is participating)
                var getUserProject = from userProject in _unitOfWork.Repository<UserProjects>().GetDbset()
                                     where userProject.UserId == validUser.UserId && userProject.ProjectId == operatedTask.ProjectId
                                     select userProject;
                if (getUserProject == null || getUserProject.Count() < 1)
                {
                    throw new TaskServiceException(TaskRelatedErrorsConstants.ACCESS_TO_TASK_IS_FORBIDDEN);
                }

                // flag to know if any field is going to be changed or not
                bool isUpdated = false;

                if (!operatedTask.Deleted)
                {
                    //First change the delete state of the task
                    operatedTask.Deleted = true;

                    // Define recursive call to set delete state of children of a task
                    var tasksDbSet = _unitOfWork.Repository<Tasks>().GetDbset();
                    async Task<bool> recursiveDeleteChildrenTasks(Tasks task)
                    {
                        if (task == null) return false;
                        // Find all tasks that have this task as parent
                        var query = tasksDbSet.Where(t => t.ParentId == task.Id);
                        if (query == null || query.Count() < 1) return true;
                        // Stop query and get results
                        var childrenTasks = query.ToList();
                        // For each of them change the project they belong to, to this new parent project
                        foreach (Tasks t in childrenTasks)
                        {
                            t.Deleted = true;
                            t.UpdatedBy = validUser.UserId;
                            t.UpdatedDate = rightNow;
                            _unitOfWork.Repository<Tasks>().Update(t);
                            await recursiveDeleteChildrenTasks(t);
                        }
                        return true;
                    }

                    // Run the recursive call
                    await recursiveDeleteChildrenTasks(operatedTask);
                    isUpdated = true;
                }

                // If there is any update, we update the object
                if (isUpdated)
                {
                    operatedTask.UpdatedBy = validUser.UserId;
                    operatedTask.UpdatedDate = rightNow;
                    _unitOfWork.Repository<Tasks>().Update(operatedTask);
                    await _unitOfWork.SaveChangesAsync();
                }

                await transaction.CommitAsync();

                return new TaskResponseModel(operatedTask);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, ErrorLoggingMessagesConstants.TASK_SERVICE_ERROR_LOG_MESSAGE);
                throw ex;
            }
        }
    }
}
