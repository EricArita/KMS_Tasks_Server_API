using Core.Application.Helper.Strategies.Participation;
using Core.Application.Interfaces;
using Core.Application.Models.Participation;
using Core.Application.Models.Participation.GETSpecificResponses;
using Core.Domain.DbEntities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Strategies.Participation
{
    public class GetProjectRoles_OfUser_InProject_Strategy : GetAllParticipationStrategy
    {
        public GetProjectRoles_OfUser_InProject_Strategy(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : base(unitOfWork, userManager)
        {
        }

        public override Task<IGetAllParticipations_ResponseModel> GetAllParticipations(GetAllParticipationsModel model)
        {
            throw new NotImplementedException();
        }
    }
}
