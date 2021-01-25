using Core.Application.Models.Participation.RelatedClasses;
using Core.Application.Models.Project;
using System.Collections.Generic;

namespace Core.Application.Models.Participation.GETSpecificResponses
{
    public class GetAllParticipatingUsers_InProject_ResponseModel : IGetAllParticipations_ResponseModel
    {
        public GetAllParticipatingUsers_InProject_ResponseModel(Domain.DbEntities.Project project, IEnumerable<UserMappedToProjectRoles> mappedRecords)
        {
            if (project == null || mappedRecords == null) return;
            Project = new ProjectResponseModel(project, null);
            Users = mappedRecords;
        }

        public ProjectResponseModel Project { get; set; }
        public IEnumerable<UserMappedToProjectRoles> Users { get; set; }
    }
}
