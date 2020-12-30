using Core.Application.Helper.Exceptions.Participation;
using Core.Application.Helper.Strategies.Participation;
using Core.Application.Interfaces;
using Core.Application.Models.Participation;
using Core.Application.Models.Participation.GETSpecificResponses;
using Core.Domain.Constants;
using Core.Domain.DbEntities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Persistence.Strategies.Participation
{
    public class GetProjectRoles_OfUser_InProject_Strategy : GetAllParticipationStrategy
    {
        public GetProjectRoles_OfUser_InProject_Strategy(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : base(unitOfWork, userManager)
        {
        }

        public override IGetAllParticipations_ResponseModel GetAllParticipations(long queriedByUserId, GetAllParticipationsModel model)
        {
            // We need to do some validations before first step 
            // One of them is : check if ProjectId is valid or not???
            Project validProject = _unitOfWork.Repository<Project>().GetDbset().FirstOrDefault(e => e.Id == model.ProjectId);
            if (validProject == null)
            {
                throw new ParticipationServiceException(ProjectRelatedErrorsConstants.PROJECT_NOT_FOUND);
            }
            // Another is : check if the user is participating in the queried project
            // We only allow them to get participations of a project if they participate
            bool queryingUserHasParticipation = _unitOfWork.Repository<UserProjects>().GetDbset().Any(p => p.ProjectId == validProject.Id && p.UserId == queriedByUserId);
            if (!queryingUserHasParticipation)
            {
                throw new ParticipationServiceException(ProjectParticipationRelatedErrorsConstants.PROJECT_PARTICIPATION_NOT_FOUND);
            }

            // Check if userId in model is valid or not
            ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == model.UserId);
            if (validUser == null)
            {
                throw new ParticipationServiceException(UserRelatedErrorsConstants.USER_NOT_FOUND);
            }

            // queriedUser has participation in queried project or not
            bool queriedUserHasParticipationInProject = _unitOfWork.Repository<UserProjects>().GetDbset().Any(p => p.ProjectId == validProject.Id && p.UserId == model.UserId);
            if (!queriedUserHasParticipationInProject)
            {
                throw new ParticipationServiceException(ProjectParticipationRelatedErrorsConstants.QUERIED_USER_HAS_NO_PARTICIPATIONS_IN_QUERIED_PROJECT);
            }

            // If all validations pass, we get the roles
            IEnumerable<ProjectRole> roles = _unitOfWork.Repository<ProjectRole>().GetDbset()
                .Where(role => _unitOfWork.Repository<UserProjects>().GetDbset()
                .Any(item => item.ProjectId == model.ProjectId && item.UserId == model.UserId && item.RoleId == role.Id));

            // Final response, formatted
            GetAllProjectRoles_OfUser_InProject_ResponseModel response =
                new GetAllProjectRoles_OfUser_InProject_ResponseModel(validProject, validUser, roles);

            return response;
        }
    }
}
