using Core.Application.Interfaces;
using Core.Application.Models.Participation;
using Core.Application.Models.Participation.GETSpecificResponses;
using Core.Domain.DbEntities;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace Core.Application.Helper.Strategies.Participation
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

        public abstract Task<IGetAllParticipations_ResponseModel> GetAllParticipations(GetAllParticipationsModel model);
    }
}
