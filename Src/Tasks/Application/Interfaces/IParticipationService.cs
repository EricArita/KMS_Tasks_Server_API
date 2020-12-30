using Core.Application.Models.Participation;
using Core.Application.Models.Participation.GETSpecificResponses;
using System.Threading.Tasks;

namespace Core.Application.Interfaces
{
    public interface IParticipationService
    {
        public Task<ParticipationResponseModel> AddNewParticipation(long createdByUserId, NewParticipationModel newParticipation);
        public Task<IGetAllParticipations_ResponseModel> GetAllParticipations(long queriedByUserId, GetAllParticipationsModel model);
        public Task<object> DeleteExistingParticipation(long deletedByUserId, DeleteParticipationModel model);
    }
}
