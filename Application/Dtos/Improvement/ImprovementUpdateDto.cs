using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
