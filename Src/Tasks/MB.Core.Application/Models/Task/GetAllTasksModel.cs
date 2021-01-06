using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MB.Core.Application.Models.Task
{
    public class GetAllTasksModel
    {
        [Required]
        public long? UserId { get; set; }

        // Queries
        public long? ProjectId { get; set; }
        public byte? CategoryType { get; set; }
    }
}
