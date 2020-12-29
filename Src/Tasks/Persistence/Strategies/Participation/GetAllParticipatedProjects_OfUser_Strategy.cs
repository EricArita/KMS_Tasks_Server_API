using Core.Application.Helper.Exceptions.Participation;
using Core.Application.Helper.Strategies.Participation;
using Core.Application.Interfaces;
using Core.Application.Models.Participation;
using Core.Application.Models.Participation.GETSpecificResponses;
using Core.Application.Models.Project;
using Core.Domain.Constants;
using Core.Domain.DbEntities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Strategies.Participation
{
    public class GetAllParticipatedProjects_OfUser_Strategy : GetAllParticipationStrategy
    {
        public GetAllParticipatedProjects_OfUser_Strategy(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : base(unitOfWork, userManager)
        {
        }

        public override IGetAllParticipations_ResponseModel GetAllParticipations(long queriedByUserId, GetAllParticipationsModel model)
        {
            // First we get the query as needed
            var queryForNeededParticipations = from participation in _unitOfWork.Repository<UserProjects>().GetDbset()
                        where participation.UserId == model.UserId
                        select participation;

            // Second, We filter the distinct projects from the first query.
            var distinctProjects = _unitOfWork.Repository<Project>().GetDbset()
                .Where(project => queryForNeededParticipations.Any(item => item.ProjectId == project.Id));
            IEnumerable<Project> resultingProjects = null;
            IQueryable<UserProjects> resultingParticipations = null;

            // However, we have to validate even though the needed query is already ran to
            // only return PROJECTS and PARTICIPATIONS IN PROJECTS that the querying user is participating in 
            // (Querying user could be different from the userId in the model)

            // This is done to effectively not let other users know what projects a certain user is participating in
            // unless they collaborate directly with that certain user. Therefore, we will only run when
            // the querying user is different from the userId in the input model
            if (queriedByUserId != model.UserId)
            {
                var newQueryForParticipation = _unitOfWork.Repository<UserProjects>()
                    .GetDbset().Where(item => distinctProjects.Any(project => project.Id == item.ProjectId && item.UserId == queriedByUserId));
                // We get the list of new projects
                var newDistinctProjects = distinctProjects
                    .Where(project => _unitOfWork.Repository<UserProjects>().GetDbset()
                    .Any(item => project.Id == item.ProjectId && item.UserId == queriedByUserId));
                resultingProjects = newDistinctProjects.ToList();
                resultingParticipations = newQueryForParticipation;
            }
            else
            {
                resultingProjects = distinctProjects.ToList();
                resultingParticipations = queryForNeededParticipations;
            }  

            // Atlast, we get the full info user from the model and get the full project info for each project found
            // from filtering the needed participations from above
            ApplicationUser validUser = _userManager.Users.FirstOrDefault(e => e.UserId == model.UserId);
            if (validUser == null)
            {
                throw new ParticipationServiceException(UserRelatedErrorsConstants.USER_NOT_FOUND);
            }

            // Final and formatted result
            List<ProjectResponseModel> actualFormattedResult = new List<ProjectResponseModel>();
            var projectRoles = _unitOfWork.Repository<ProjectRole>().GetDbset();
            foreach (var project in resultingProjects)
            {
                // get the roles for this project
                var roles = projectRoles.Where(role => resultingParticipations.Any(p => p.ProjectId == project.Id && p.RoleId == role.Id));
                actualFormattedResult.Add(new ProjectResponseModel(project, roles));
            };

            GetAllParticipatedProjects_OfUser_ResponseModel response = new GetAllParticipatedProjects_OfUser_ResponseModel(validUser, actualFormattedResult);

            return response;
        }
    }
}
