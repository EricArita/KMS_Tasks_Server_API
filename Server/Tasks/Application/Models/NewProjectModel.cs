using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Application.Models
{
    public class NewProjectModel
    {
        [Required]
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public string Description { get; set; }
        public long? CreatedBy { get; set; }
    }
}
