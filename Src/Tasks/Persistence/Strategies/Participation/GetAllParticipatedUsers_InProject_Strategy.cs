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
    public class GetAllParticipatedUsers_InProject_Strategy : GetAllParticipationStrategy
    {
        public GetAllParticipatedUsers_InProject_Strategy(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : base(unitOfWork, userManager)
        {
        }

        public override IGetAllParticipations_ResponseModel GetAllParticipations(long queriedByUserId, GetAllParticipationsModel model)
        {
            throw new NotImplementedException();
        }
    }
}
