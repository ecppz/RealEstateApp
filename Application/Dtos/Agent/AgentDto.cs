using Domain.Common.Enums;

namespace Application.Dtos.Agent
{
    public class AgentDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; } 
        public required string LastName { get; set; } 
        public required string Email { get; set; }
        public required int PropertyCount { get; set; }
        public string? Phone { get; set; }
        public string? ProfileImage { get; set; }
        public UserStatus Status { get; set; }
        public Roles Role { get; set; }
    }
}
