using Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Example : BaseEntity
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
