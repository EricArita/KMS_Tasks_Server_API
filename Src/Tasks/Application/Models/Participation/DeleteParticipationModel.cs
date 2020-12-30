using Core.Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Application.Models.Participation
{
    public class DeleteParticipationModel
    {
        [Required(ErrorMessage = "You need to provide a project Id to remove the participation from")]
        public long? RemoveFromProjectId { get; set; }
        [Required(ErrorMessage = "You need to provide a user Id to remove their participation from the project")]
        public long? RemoveUserId { get; set; }
        [EnumDataType(typeof(Enums.ProjectRoles))]
        public Enums.ProjectRoles? RemoveProjectRoleId { get; set; }
    }
}
