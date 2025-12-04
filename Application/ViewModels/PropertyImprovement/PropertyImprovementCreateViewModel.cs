using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.PropertyImprovement
{
    public class PropertyImprovementCreateViewModel
    {
        [Required(ErrorMessage = "El Id de la propiedad es obligatorio")]
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "El Id de la mejora es obligatorio")]
        public int ImprovementId { get; set; }
    }
}
