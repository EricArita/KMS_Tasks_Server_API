using Core.Application.DTOs;
using Core.Application.Models.Project;
using Core.Domain.DbEntities;
using System.Collections.Generic;

namespace Core.Application.Models.Participation
{
    public class ParticipationResponseModel
    {
        public ParticipationResponseModel(UserProjects participation)
        {
            if (participation == null) return;
            User = new UserDTO(participation.User);
            List<ProjectRole> roles = new List<ProjectRole>();
            roles.Add(participation.ProjectRole);
            ParticipatedProject = new ProjectResponseModel(participation.Project, roles);
        }

        public UserDTO User { get; set; }
        public ProjectResponseModel ParticipatedProject { get; set; }
    }
}
