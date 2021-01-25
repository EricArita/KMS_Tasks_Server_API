using MB.Core.Domain.DbEntities;
using MB.Core.Application.DTOs;
using System;
using System.Collections.Generic;

namespace MB.Core.Application.Models.Project
{
    public class ProjectResponseModel
    {
        public ProjectResponseModel(Domain.DbEntities.Project project, IEnumerable<ProjectRole> roles)
        {
            if (project == null) return;
            Id = project.Id;
            Name = project.Name;
            Description = project.Description;
            CreatedDate = project.CreatedDate;
            CreatedBy = project.CreatedByUser == null ? null : new UserDTO(project.CreatedByUser);
            UpdatedDate = project.UpdatedDate;
            UpdatedBy = project.UpdatedByUser == null ? null : new UserDTO(project.UpdatedByUser);
            if (roles != null)
            {
                ProjectRoles = roles;
            }
            IsDeleted = project.Deleted;
            if (project.Parent == null) return;
            Parent = new ProjectResponseModel(project.Parent, null);
        }

        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedDate { get; set; }
        public UserDTO CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public UserDTO UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public ProjectResponseModel Parent { get; set; }
        public IEnumerable<ProjectRole> ProjectRoles { get; set; }
    }
}
