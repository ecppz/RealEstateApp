using Domain.Common.Enums;

namespace Application.Dtos.Agent
{
    public class EditAgentDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; } 
        public required string LastName { get; set; } 
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required string PhoneNumber { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? ProfileImage { get; set; }
        public UserStatus Status { get; set; }
        public Roles Role { get; set; }
    }
}
