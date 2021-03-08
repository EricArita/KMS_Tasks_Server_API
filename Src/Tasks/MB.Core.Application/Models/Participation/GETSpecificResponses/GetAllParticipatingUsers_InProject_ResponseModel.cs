using MB.Core.Application.Models.Participation.RelatedClasses;
using MB.Core.Application.Models.Project;
using System.Collections.Generic;

namespace MB.Core.Application.Models.Participation.GETSpecificResponses
{
    public class GetAllParticipatingUsers_InProject_ResponseModel : IGetAllParticipations_ResponseModel
    {
        public GetAllParticipatingUsers_InProject_ResponseModel(Domain.DbEntities.Project project, List<UserMappedToProjectRoles> mappedRecords)
        {
            if (project == null || mappedRecords == null) return;
            Project = new ProjectResponseModel(project, null, null, null);
            Users = mappedRecords;
        }

        public ProjectResponseModel Project { get; set; }
        public List<UserMappedToProjectRoles> Users { get; set; }
    }
}
