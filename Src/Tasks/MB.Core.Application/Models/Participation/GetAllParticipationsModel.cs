using Core.Application.Models.Utils;
using Core.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Models.Participation
{
    [AtLeastOneFieldRequired(ErrorMessage = "You need to supply at least one field to query participations")]
    public class GetAllParticipationsModel
    {
        public long? UserId { get; set; }
        public long? ProjectId { get; set; }
    }
}
