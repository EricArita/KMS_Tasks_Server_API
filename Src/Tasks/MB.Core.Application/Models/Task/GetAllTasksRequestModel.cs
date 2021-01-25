using MB.Core.Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace MB.Core.Application.Models.Task
{
    public class GetAllTasksRequestModel
    {
        // Queries, must provide at least one
        public long? ProjectId { get; set; }
        [EnumDataType(typeof(Enums.MenuSidebarOptions))]
        public Enums.MenuSidebarOptions? CategoryType { get; set; }
    }
}
