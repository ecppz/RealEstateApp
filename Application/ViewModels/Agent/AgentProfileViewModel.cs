using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.Agent
{
    public class AgentProfileViewModel
    {
        public required string Id { get; set; }

        [Required(ErrorMessage = "Debes ingresar el nombre del agente")]
        [DataType(DataType.Text)]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Debes ingresar el apellido del agente")]
        [DataType(DataType.Text)]
        public required string LastName { get; set; }

        [Required(ErrorMessage = "Debes ingresar el telefono del agente")]
        [DataType(DataType.Text)]
        public required string PhoneNumber { get; set; }

        [DataType(DataType.Upload)]
        public IFormFile? ProfileImageFile { get; set; }
    }
}
