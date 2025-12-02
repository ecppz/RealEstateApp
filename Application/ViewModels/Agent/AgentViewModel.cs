using Domain.Common.Enums;

namespace Application.ViewModels.Agent
{
    public class AgentViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? ProfileImage { get; set; }
        public int PropertyCount { get; set; }
        public UserStatus Status { get; set; }
    }
}
