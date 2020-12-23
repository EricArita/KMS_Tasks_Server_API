using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Application.Models
{
    public class GetOneProjectModel
    {
        [Required]
        public long? ProjectId { get; set; }
        [Required]
        public long? UserId { get; set; }
    }
}
