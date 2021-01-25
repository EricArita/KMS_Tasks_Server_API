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
    }
}
