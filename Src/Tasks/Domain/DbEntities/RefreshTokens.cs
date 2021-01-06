using Microsoft.EntityFrameworkCore;
using System;

namespace MB.Core.Domain.DbEntities
{
    [Owned]
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime ExpiredDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? RevokedDate { get; set; }
        public bool IsExpired => DateTime.UtcNow > ExpiredDate;
    }
}
