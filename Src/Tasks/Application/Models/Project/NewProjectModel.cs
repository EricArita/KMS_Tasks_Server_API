using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MB.Core.Application.Models
{
    public class NewProjectModel
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Project's {0} is required")]
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public string Description { get; set; }
    }
}
