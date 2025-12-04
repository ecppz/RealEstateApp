
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.PropertyType
{
    public class PropertyTypeCreateViewModel
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es requerida")]
        public string Description { get; set; } = string.Empty;
    }
}
