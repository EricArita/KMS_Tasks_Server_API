using MB.Core.Application.Models.Participation;
using MB.Core.Application.Models.Participation.GETSpecificResponses;
using System.Threading.Tasks;

namespace MB.Core.Application.Interfaces
{
    public interface IParticipationService
    {
        public Task<ParticipationResponseModel> AddNewParticipation(long createdByUserId, NewParticipationModel newParticipation);
        public Task<IGetAllParticipations_ResponseModel> GetAllParticipations(long queriedByUserId, GetAllParticipationsModel model);
        public Task<object> DeleteExistingParticipation(long deletedByUserId, DeleteParticipationModel model);
    }
}
