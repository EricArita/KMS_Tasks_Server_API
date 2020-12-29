﻿using Core.Domain.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.Application.Models.Task
{
    public class GetAllTasksModel
    {
        [Required]
        public long? UserId { get; set; }

        // Queries
        public long? ProjectId { get; set; }
        [EnumDataType(typeof(Enums.MenuSidebarOptions))]
        public Enums.MenuSidebarOptions? CategoryType { get; set; }
    }
}
