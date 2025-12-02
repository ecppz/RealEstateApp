using Domain.Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity.Entities
{
    public class UserAccount : IdentityUser
    {
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public string? ProfileImage { get; set; }
        public string? DocumentNumber { get; set; }
        public required UserStatus Status { get; set; }
    }
}
