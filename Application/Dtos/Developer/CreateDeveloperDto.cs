using Domain.Common.Enums;

namespace Application.Dtos.Developer
{
    public class CreateDeveloperDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; } 
        public required string LastName { get; set; } 
        public required string DocumentNumber { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; } 
        public required string Password { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;
        public Roles Role { get; set; } = Roles.Developer;
    }
}
