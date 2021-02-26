using System;
using System.Collections.Generic;
using System.Text;

namespace MB.Core.Application.Models.Project
{
    public class GetAllProjectsResponseModel
    {
        public GetAllProjectsResponseModel(IEnumerable<ProjectResponseModel> projects, int totalPages)
        {
            Projects = projects;
            TotalPages = totalPages;
        }

        public IEnumerable<ProjectResponseModel> Projects { get; set; }
        public int TotalPages { get; set; }
    }
}
