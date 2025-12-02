using Domain.Common.Enums;

namespace Application.ViewModels.Agent
{
    public class AgentViewModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required string LastName { get; set; }
        public required string Email { get; set; }
        public required string UserName { get; set; }
        public required int PropertyCount { get; set; }
        public required string Phone { get; set; }
        public required string? ProfileImage { get; set; }
        public UserStatus Status { get; set; }
        public Roles Role { get; set; }
    }
}
