using MB.Core.Application.DTOs;
using MB.Core.Application.Models.Project;
using MB.Core.Domain.DbEntities;
using System.Collections.Generic;

namespace MB.Core.Application.Models.Participation
{
    public class ParticipationResponseModel
    {
        public ParticipationResponseModel(UserProjects participation)
        {
            if (participation == null) return;
            User = new UserDTO(participation.User);
            List<ProjectRole> roles = new List<ProjectRole>
            {
                participation.ProjectRole
            };
            ParticipatedProject = new ProjectResponseModel(participation.Project, roles);
        }

        public UserDTO User { get; set; }
        public ProjectResponseModel ParticipatedProject { get; set; }
    }
}
