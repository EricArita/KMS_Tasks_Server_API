using Core.Application.Helper.Strategies.Participation;
using Core.Application.Interfaces;
using Core.Application.Models.Participation;
using Core.Application.Models.Participation.GETSpecificResponses;
using Core.Domain.Constants;
using Core.Domain.DbEntities;
using Infrastructure.Persistence.Strategies.Participation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Services
{
    public class ParticipationService : IParticipationService
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected ILogger<TaskService> _logger;
        protected readonly UserManager<ApplicationUser> _userManager;

        public ParticipationService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ILogger<TaskService> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<ParticipationResponseModel> AddNewParticipation(long createdByUserId, NewParticipationModel newParticipation)
        {
            throw new NotImplementedException();
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

                switch(fieldsProvided)
                {
                    // Strategy to get all participated projects of a user, and his roles in each of them
                    case (int)Enums.GetAllParticipationsStrategy.GetAllParticipatedProjects_OfUser:
                        strategy = new GetAllParticipatedProjects_OfUser_Strategy(_unitOfWork, _userManager);
                        break;
                    // Strategy to get all participated users of a project, and the roles of each of them in the project
                    case (int)Enums.GetAllParticipationsStrategy.GetAllParticipatedUsers_InProject:
                        strategy = new GetAllParticipatedUsers_InProject_Strategy(_unitOfWork, _userManager);
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

        public Task<ParticipationResponseModel> DeleteExistingParticipation(long deletedByUserId, DeleteParticipationModel model)
        {
            throw new NotImplementedException();
        }
    }
}
