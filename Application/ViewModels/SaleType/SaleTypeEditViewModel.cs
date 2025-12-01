using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.SaleType
{
    public class SaleTypeEditViewModel
    {
        public required int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public required string Description { get; set; }
    }
}
