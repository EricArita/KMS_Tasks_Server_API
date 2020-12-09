using Core.Domain.Constants;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Models.Project
{
    public class UpdateProjectInfoModel
    {
        public string Name { get; set; }
        public long? ParentId { get; set; }
        public string Description { get; set; }
        public long? CreatedBy { get; set; }
        public bool? MakeParentless { get; set; }
    }
}
