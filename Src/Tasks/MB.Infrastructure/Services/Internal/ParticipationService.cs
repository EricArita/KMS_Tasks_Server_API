using MB.Core.Application.Helper.Strategies.Participation;
using MB.Core.Application.Interfaces;
using MB.Core.Application.Models.Participation;
using MB.Core.Application.Models.Participation.GETSpecificResponses;
using MB.Core.Domain.Constants;
using MB.Core.Domain.DbEntities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using System.Linq;
using MB.Core.Application.Helper.Exceptions.Participation;
using MB.Infrastructure.Strategies.Participation;

namespace MB.Infrastructure.Services.Internal
{
    public class ParticipationService : IParticipationService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected ILogger<ParticipationService> _logger;
        protected readonly UserManager<ApplicationUser> _userManager;

        public ParticipationService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ILogger<ParticipationService> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<ParticipationResponseModel> AddNewParticipation(long createdByUserId, NewParticipationModel newParticipation)
        {
            if (newParticipation.RoleId <= Enums.ProjectRoles.None )
            {
                throw new ParticipationServiceException(ProjectParticipationRelatedErrorsConstants.CANNOT_CREATE_PARTICIPATION_WITH_NONE_AS_A_ROLE);
            }

            if (newParticipation.RoleId == Enums.ProjectRoles.Owner)
            {
                throw new ParticipationServiceException(ProjectParticipationRelatedErrorsConstants.CANNOT_CREATE_PARTICIPATION_WITH_OWNER_AS_A_ROLE);
            }

            await using var transaction = await _unitOfWork.CreateTransaction();

            try
            {
                // Check if userId in model is valid or not
                ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == newParticipation.UserId);
                if (validUser == null)
                {
                    throw new ParticipationServiceException(UserRelatedErrorsConstants.USER_NOT_FOUND);
                }

                // Check if ProjectId is valid or not
                Project validProject = _unitOfWork.Repository<Project>().GetDbset().FirstOrDefault(e => e.Id == newParticipation.ProjectId);
                if (validProject == null)
                {
                    throw new ParticipationServiceException(ProjectRelatedErrorsConstants.PROJECT_NOT_FOUND);
                }

                // In the future, we will check if the user creating the participation have the rights to create one
                // Preferably, we want only the owner, PM or leader to have the rights
                bool creatingUserHaveTheRights = _unitOfWork.Repository<UserProjects>().GetDbset()
                    .Any(item =>
                    // Check if the creator have the rights
                    item.ProjectId == validProject.Id && item.UserId == createdByUserId &&
                    item.RoleId >= Enums.ProjectRoles.Owner && item.RoleId <= Enums.ProjectRoles.Leader);
                if (!creatingUserHaveTheRights)
                {
                    throw new ParticipationServiceException(ProjectParticipationRelatedErrorsConstants.PARTICIPATION_CREATOR_DONT_HAVE_THE_RIGHTS);
                }

                // We continue to check if the new participation for the userId already exists or not
                bool newParticipationAlreadyExists = _unitOfWork.Repository<UserProjects>().GetDbset()
                    .Any(item => item.ProjectId == validProject.Id && item.UserId == validUser.UserId && item.RoleId == newParticipation.RoleId);
                if (newParticipationAlreadyExists)
                {
                    throw new ParticipationServiceException(ProjectParticipationRelatedErrorsConstants.CANNOT_RECREATE_AN_EXISTING_PARTICIPATION);
                }

                // If everything is fine, we insert the participation
                UserProjects participation = new UserProjects()
                {
                    ProjectId = validProject.Id,
                    UserId = validUser.UserId,
                    RoleId = newParticipation.RoleId.Value
                };

                await _unitOfWork.Repository<UserProjects>().InsertAsync(participation);

                await _unitOfWork.SaveChangesAsync();

                // Eager load instance for initialization of response model
                var entry = _unitOfWork.Entry(participation);
                await entry.Reference(e => e.ProjectRole).LoadAsync();

                await transaction.CommitAsync();

                return new ParticipationResponseModel(participation);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, ErrorLoggingMessagesConstants.PARTICIPATION_SERVICE_ERROR_LOG_MESSAGE);
                throw ex;
            }
        }

        public async Task<IGetAllParticipations_ResponseModel> GetAllParticipations(long queriedByUserId, GetAllParticipationsModel model)
        {
            await using var transaction = await _unitOfWork.CreateTransaction();

            try
            {
                // We use different strategies for each type of get participations request
                // Each strategy will implement its own way of data validation and returns a different response structure
                int fieldsProvided = 0;
                if (model.UserId != null)
                {
                    fieldsProvided |= (1 << 0);
                }
                if (model.ProjectId != null)
                {
                    fieldsProvided |= (1 << 1);
                }

                GetAllParticipationStrategy strategy = null;

                switch (fieldsProvided)
                {
                    // Strategy to get all participated projects of a user, and his roles in each of them
                    case (int)Enums.GetAllParticipationsStrategy.GetAllParticipatedProjects_OfUser:
                        strategy = new GetAllParticipatedProjects_OfUser_Strategy(_unitOfWork, _userManager);
                        break;
                    // Strategy to get all participated users of a project, and the roles of each of them in the project
                    case (int)Enums.GetAllParticipationsStrategy.GetAllParticipatedUsers_InProject:
                        strategy = new GetAllParticipatingUsers_InProject_Strategy(_unitOfWork, _userManager);
                        break;
                    // Strategy to get all roles of a user inside a certain project
                    case (int)Enums.GetAllParticipationsStrategy.GetAllProjectRoles_OfUser_InProject:
                        strategy = new GetProjectRoles_OfUser_InProject_Strategy(_unitOfWork, _userManager);
                        break;
                }
                IGetAllParticipations_ResponseModel result = null;
                if (strategy != null)
                {
                    result = strategy.GetAllParticipations(queriedByUserId, model);
                }
                else
                {
                    throw new Exception(InternalServerErrorsConstants.GET_ALL_PARTICIPATIONS_STRATEGY_INVALID);
                }

                await _unitOfWork.SaveChangesAsync();

                await transaction.CommitAsync();

                return result;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, ErrorLoggingMessagesConstants.PARTICIPATION_SERVICE_ERROR_LOG_MESSAGE);
                throw ex;
            }
        }

        public async Task<object> DeleteExistingParticipation(long deletedByUserId, DeleteParticipationModel model)
        {
            if (model.RemoveProjectRoleId != null && model.RemoveProjectRoleId <= Enums.ProjectRoles.None)
            {
                throw new ParticipationServiceException(ProjectParticipationRelatedErrorsConstants.THERE_IS_NO_PARTICIPATION_WITH_A_NONE_ROLE);
            }

            if (model.RemoveProjectRoleId != null && model.RemoveProjectRoleId == Enums.ProjectRoles.Owner)
            {
                throw new ParticipationServiceException(ProjectParticipationRelatedErrorsConstants.CANNOT_REMOVE_THE_OWNER_FROM_HIS_OWN_PROJECT);
            }

            await using var transaction = await _unitOfWork.CreateTransaction();

            try
            {
                // Check if userId in model is valid or not
                ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == model.RemoveUserId);
                if (validUser == null)
                {
                    throw new ParticipationServiceException(UserRelatedErrorsConstants.USER_NOT_FOUND);
                }

                // Check if ProjectId is valid or not
                Project validProject = _unitOfWork.Repository<Project>().GetDbset().FirstOrDefault(e => e.Id == model.RemoveFromProjectId);
                if (validProject == null)
                {
                    throw new ParticipationServiceException(ProjectRelatedErrorsConstants.PROJECT_NOT_FOUND);
                }

                // Check if user have the rights to delete the participation
                // Only Owner, PM and leader can delete a participation of a project
                bool removingUserHaveTheRights = _unitOfWork.Repository<UserProjects>().GetDbset()
                    .Any(item =>
                    // Check if the creator have the rights
                    item.ProjectId == validProject.Id && item.UserId == deletedByUserId &&
                    item.RoleId >= Enums.ProjectRoles.Owner && item.RoleId <= Enums.ProjectRoles.Leader);
                if (!removingUserHaveTheRights)
                {
                    throw new ParticipationServiceException(ProjectParticipationRelatedErrorsConstants.PARTICIPATION_REMOVER_DONT_HAVE_THE_RIGHTS);
                }

                var matchingParticipations = _unitOfWork.Repository<UserProjects>().GetDbset()
                    .Where(item => item.ProjectId == validProject.Id && item.UserId == validUser.UserId);
                
                // If the user leave the removeProjectRoleId blank, we will infere that the deleting user wants to remove
                // the user indicated by the userId in the model, we have to check if they want to remove the Owner or not
                // Removal of the owner is not allowed
                if (model.RemoveProjectRoleId == null)
                {
                    bool isTheRemovedUserOwner = matchingParticipations.Any(item => item.RoleId == Enums.ProjectRoles.Owner);
                    if (isTheRemovedUserOwner)
                    {
                        throw new ParticipationServiceException(ProjectParticipationRelatedErrorsConstants.CANNOT_REMOVE_THE_OWNER_FROM_HIS_OWN_PROJECT);
                    }
                }

                // We continue to get the participation(s) that the user want to remove
                var existingParticipations =  matchingParticipations.Where(item => (model.RemoveProjectRoleId == null || item.RoleId == model.RemoveProjectRoleId));
                if (existingParticipations == null || existingParticipations.Count() <= 0)
                {
                    throw new ParticipationServiceException(ProjectParticipationRelatedErrorsConstants.CANNOT_LOCATE_AN_EXISTING_PARTICIPATION_FOR_REMOVAL);
                }

                // If we can locate one or many participations to remove, remove
                foreach(var participation in existingParticipations)
                {
                    _unitOfWork.Repository<UserProjects>().DeleteByObject(participation);
                }

                await _unitOfWork.SaveChangesAsync();

                await transaction.CommitAsync();

                return null;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, ErrorLoggingMessagesConstants.PARTICIPATION_SERVICE_ERROR_LOG_MESSAGE);
                throw ex;
            }
        }
    }
}
