using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Improvement
{
    public class ImprovementUpdateDto
    {
        public required int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la mejora es obligatorio")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "La descripción de la mejora es obligatoria")]
        public required string Description { get; set; }
    }
}
