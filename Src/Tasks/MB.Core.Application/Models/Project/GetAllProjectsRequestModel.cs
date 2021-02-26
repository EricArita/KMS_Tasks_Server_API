using System;
using System.Collections.Generic;
using System.Text;

namespace MB.Core.Application.Models.Project
{
    public class GetAllProjectsRequestModel
    {
        // Queries
        public string ProjectName { get; set; }

        // Get projects by page
        public int? PageNumber { get; set; }
        public int? ItemPerPage { get; set; }
    }
}
