using MB.Core.Domain.DbEntities;
using MB.Core.Application.DTOs;
using System;
using System.Collections.Generic;
using MB.Core.Application.Models.Task;

namespace MB.Core.Application.Models.Project
{
    public class ProjectResponseModel
    {
        public ProjectResponseModel(Domain.DbEntities.Project project, IEnumerable<ProjectRole> roles, IEnumerable<ProjectResponseModel> children, IEnumerable<TaskResponseModel> childrenTasks)
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
            if (project.Parent != null) {
                Parent = new ProjectResponseModel(project.Parent, null, null, null);
            }
            if (project.Children != null)
            {
                Children = children;
            }
            if (project.ChildrenTasks != null)
            {
                ChildrenTasks = childrenTasks;
            }
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
        public IEnumerable<ProjectResponseModel> Children { get; set; }
        public IEnumerable<TaskResponseModel> ChildrenTasks { get; set; }
    }
}
