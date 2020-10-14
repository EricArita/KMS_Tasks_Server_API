using Microsoft.AspNetCore.Identity;
using System;

namespace Core.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }
        public bool Active { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow;
    }
}
