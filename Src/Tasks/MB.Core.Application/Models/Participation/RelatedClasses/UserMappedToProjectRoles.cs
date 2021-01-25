using MB.Core.Application.DTOs;
using MB.Core.Domain.DbEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MB.Core.Application.Models.Participation.RelatedClasses
{
    public class UserMappedToProjectRoles
    {
        public UserMappedToProjectRoles(ApplicationUser user, IEnumerable<ProjectRole> roles)
        {
            if (user == null || roles == null) return;
            UserDetail = new UserDTO(user);
            RolesInProject = roles;
        }
        public UserDTO UserDetail { get; set; }
        public IEnumerable<ProjectRole> RolesInProject { get; set; }
    }
}
