using Domain.Common.Enums;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.Developer
{
    public class CreateDeveloperViewModel
    {
        public string? Id { get; set; }
        [Required(ErrorMessage = "Debes ingresar el nombre del desarrollador")]
        [DataType(DataType.Text)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Debes ingresar el apellido del desarrollador")]
        [DataType(DataType.Text)]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Debes ingresar la cédula del administrador")]
        [RegularExpression(@"^\d{3}-\d{7}-\d{1}$|^\d{11}$", ErrorMessage = "Formato de cédula inválido")]
        [DataType(DataType.Text)]
        public required string DocumentNumber { get; set; }

        [Required(ErrorMessage = "Debes ingresar el correo del desarrollador")]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required(ErrorMessage = "Debes ingresar el nombre de usuario")]
        [DataType(DataType.Text)]
        public required string UserName { get; set; }

        [Required(ErrorMessage = "Debes ingresar la contraseña")]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [Required(ErrorMessage = "Debes confirmar la contraseña")]
        [Compare(nameof(Password), ErrorMessage = "Las contraseñas no coinciden")]
        [DataType(DataType.Password)]
        public required string ConfirmPassword { get; set; }
        public UserStatus Status { get; set; } = UserStatus.Active;
        public Roles Role { get; set; } = Roles.Developer;
    }
}
