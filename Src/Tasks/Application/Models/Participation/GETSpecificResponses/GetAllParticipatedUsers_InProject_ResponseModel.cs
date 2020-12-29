using Core.Application.Models.Participation.RelatedClasses;
using System.Collections.Generic;

namespace Core.Application.Models.Participation.GETSpecificResponses
{
    public class GetAllParticipatedUsers_InProject_ResponseModel : IGetAllParticipations_ResponseModel
    {
        public GetAllParticipatedUsers_InProject_ResponseModel(Domain.DbEntities.Project project, IEnumerable<UserMappedToProjectRoles> mappedRecords)
        {
            if (project == null || mappedRecords == null) return;
            Project = project;
            Users = mappedRecords;
        }

        public Domain.DbEntities.Project Project { get; set; }
        public IEnumerable<UserMappedToProjectRoles> Users { get; set; }
    }
}
