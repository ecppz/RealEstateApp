using Domain.Common.Enums;

namespace Application.Dtos.Developer
{
    public class EditDeveloperDto
    {
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty; 
        public string Email { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;
        public Roles Role { get; set; } = Roles.Developer;
    }
}
