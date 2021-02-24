using MB.Core.Application.DTOs;
using MB.Core.Application.Models.Project;
using MB.Core.Domain.DbEntities;
using System.Collections.Generic;

namespace MB.Core.Application.Models.Participation.GETSpecificResponses
{
    public class GetAllProjectRoles_OfUser_InProject_ResponseModel : IGetAllParticipations_ResponseModel
    {
        public GetAllProjectRoles_OfUser_InProject_ResponseModel(Domain.DbEntities.Project project, ApplicationUser user, IEnumerable<ProjectRole> roles)
        {
            if (project == null || user == null || roles == null) return;
            Project = new ProjectResponseModel(project, roles, null, null);
            User = new UserDTO(user);
        }

        public ProjectResponseModel Project { get; set; }
        public UserDTO User { get; set; }
    }
}
