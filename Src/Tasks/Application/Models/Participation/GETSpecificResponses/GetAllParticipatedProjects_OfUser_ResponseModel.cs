using Core.Application.DTOs;
using Core.Application.Models.Project;
using Core.Domain.DbEntities;
using System.Collections.Generic;

namespace Core.Application.Models.Participation.GETSpecificResponses
{
    public class GetAllParticipatedProjects_OfUser_ResponseModel : IGetAllParticipations_ResponseModel
    {
        public GetAllParticipatedProjects_OfUser_ResponseModel(ApplicationUser user, IEnumerable<ProjectResponseModel> participatedProjectsWithRoles)
        {
            if (user == null || participatedProjectsWithRoles == null) return;
            User = new UserDTO(user);
            ParticipatedProjects = participatedProjectsWithRoles;
        }

        public UserDTO User { get; set; }

        public IEnumerable<ProjectResponseModel> ParticipatedProjects { get; set; }
    }
}
