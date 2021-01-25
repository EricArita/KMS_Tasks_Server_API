using MB.Core.Application.Interfaces;
using MB.Core.Application.Models.Participation;
using MB.Core.Application.Models.Participation.GETSpecificResponses;
using MB.Core.Domain.DbEntities;
using Microsoft.AspNetCore.Identity;

namespace MB.Core.Application.Helper.Strategies.Participation
{
    public abstract class GetAllParticipationStrategy
    {
        protected IUnitOfWork _unitOfWork;
        protected UserManager<ApplicationUser> _userManager;

        protected GetAllParticipationStrategy(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public abstract IGetAllParticipations_ResponseModel GetAllParticipations(long queriedByUserId, GetAllParticipationsModel model);
    }
}
