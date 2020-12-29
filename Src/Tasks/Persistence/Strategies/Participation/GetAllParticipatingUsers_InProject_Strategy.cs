using Core.Application.Helper.Exceptions.Participation;
using Core.Application.Helper.Strategies.Participation;
using Core.Application.Interfaces;
using Core.Application.Models.Participation;
using Core.Application.Models.Participation.GETSpecificResponses;
using Core.Application.Models.Participation.RelatedClasses;
using Core.Domain.Constants;
using Core.Domain.DbEntities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Persistence.Strategies.Participation
{
    public class GetAllParticipatingUsers_InProject_Strategy : GetAllParticipationStrategy
    {
        public GetAllParticipatingUsers_InProject_Strategy(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : base(unitOfWork, userManager)
        {
        }

        public override IGetAllParticipations_ResponseModel GetAllParticipations(long queriedByUserId, GetAllParticipationsModel model)
        {
            // We need to do some validations before first step 
            // One of them is : check if ProjectId is valid or not???
            Project validProject = _unitOfWork.Repository<Project>().GetDbset().FirstOrDefault(e => e.Id == model.ProjectId);
            if(validProject == null)
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

            // First we get the query as needed: get the participations in projectId
            var queryForNeededParticipations = from participation in _unitOfWork.Repository<UserProjects>().GetDbset()
                                               where participation.ProjectId == model.ProjectId
                                               select participation;

            // Second, We filter the distinct users from the first query (one user might have many roles)
            var distinctUsers = _userManager.Users
                .Where(user => queryForNeededParticipations.Any(item => item.UserId == user.UserId));
            IEnumerable<ApplicationUser> resultingUsers = distinctUsers.ToList();

            // Atlast, we get the full user info along with his/her roles in the project for each user found
            // from filtering the needed participations from above

            // Final and formatted result
            List<UserMappedToProjectRoles> actualFormattedResult = new List<UserMappedToProjectRoles>();
            var projectRoles = _unitOfWork.Repository<ProjectRole>().GetDbset();
            foreach (var user in resultingUsers)
            {
                // get the roles for this user in this project
                var roles = projectRoles.Where(role => queryForNeededParticipations.Any(p => p.UserId == user.UserId && p.RoleId == role.Id));
                actualFormattedResult.Add(new UserMappedToProjectRoles(user, roles));
            };

            GetAllParticipatingUsers_InProject_ResponseModel response = new GetAllParticipatingUsers_InProject_ResponseModel(validProject, actualFormattedResult);

            return response;
        }
    }
}
