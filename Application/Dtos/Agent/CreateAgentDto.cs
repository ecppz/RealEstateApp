using Domain.Common.Enums;

namespace Application.Dtos.Agent
{
    public class CreateAgentDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; } 
        public required string LastName { get; set; } 
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string PhoneNumber { get; set; }
        public required string Password { get; set; }
        public required string ProfileImage { get; set; }
        public UserStatus Status { get; set; }
        public Roles Role { get; set; }
    }
}
