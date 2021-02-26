using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MB.Core.Application.Models
{
    public class GetAllProjectsModel
    {
        [Required]
        public long? UserID { get; set; }
        public string ProjectName { get; set; }

        // Get projects by page
        public int? PageNumber { get; set; }
        public int? ItemPerPage { get; set; }
    }
}
