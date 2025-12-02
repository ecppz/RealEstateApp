using Domain.Common.Enums;

namespace Application.ViewModels.Agent
{
    public class DeactivateAgentViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int PropertyCount { get; set; }
        public UserStatus Status { get; set; }
    }

}
