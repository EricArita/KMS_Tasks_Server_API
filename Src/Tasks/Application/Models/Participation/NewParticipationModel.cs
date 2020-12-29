using Core.Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Application.Models.Participation
{
    public class NewParticipationModel
    {
        [Required(ErrorMessage = "{0} needs to be provided to show user's participation in which project")]
        public long? ProjectId { get; set; }

        [Required(ErrorMessage = "{0} needs to be provided to show project's participation belongs to which user")]
        public long? UserId { get; set; }

        [EnumDataType(typeof(Enums.ProjectRoles))]
        public Enums.ProjectRoles? RoleId { get; set; }
    }
}
