using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.PropertyImprovement
{
    public class PropertyImprovementUpdateDto
    {
        [Required(ErrorMessage = "El Id de la relación es obligatorio")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El Id de la propiedad es obligatorio")]
        public int PropertyId { get; set; }

        [Required(ErrorMessage = "El Id de la mejora es obligatorio")]
        public int ImprovementId { get; set; }
    }
}
