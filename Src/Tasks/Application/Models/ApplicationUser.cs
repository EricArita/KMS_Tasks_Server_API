using Microsoft.AspNetCore.Identity;
using System;

namespace Core.Application.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string LoginProvider { get; set; }
        public string ProviderKey { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public string LastName { get; set; }
        public string AvatarUrl { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public byte Status { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
    }
}
