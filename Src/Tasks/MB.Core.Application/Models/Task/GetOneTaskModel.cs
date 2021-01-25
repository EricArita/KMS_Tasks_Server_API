using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MB.Core.Application.Models.Task
{
    public class GetOneTaskModel
    {
        [Required]
        public long? TaskId { get; set; }
        [Required]
        public long? UserId { get; set; }
    }
}
