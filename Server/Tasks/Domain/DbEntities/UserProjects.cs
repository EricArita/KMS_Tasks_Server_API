using System;
using System.Collections.Generic;

namespace Core.Domain.DbEntities
{
    public partial class UserProjects
    {
        public string UserId { get; set; }
        public int ProjectId { get; set; }

        public virtual Project Project { get; set; }
    }
}
