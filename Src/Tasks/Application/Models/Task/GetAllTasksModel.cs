using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Models.Task
{
    public class GetAllTasksModel
    {
        // Must not be in request query, is illegal
        public long? UserId { get; set; }

        // Queries, must provide at least one
        public long? ProjectId { get; set; }
        public byte? CategoryType { get; set; }
    }
}
