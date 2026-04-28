using Domain.Common.Enums;

namespace Application.Dtos.Developer
{
    public class DeveloperDto
    {
        public required string Id { get; set; } 
        public required string Name { get; set; } 
        public required string LastName { get; set; } 
        public required string UserName { get; set; } 
        public required string DocumentNumber { get; set; }
        public required string Email { get; set; }
        public UserStatus Status { get; set; }
        public Roles Role { get; set; } 
    }
}
