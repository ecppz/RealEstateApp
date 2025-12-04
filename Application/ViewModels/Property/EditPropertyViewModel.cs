using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Application.ViewModels.Property
{
    public class EditPropertyViewModel
    {
        [Required]
        public int Id { get; set; } 
        [Required]
        [RegularExpression(@"^\d{6}$", ErrorMessage = "El código debe contener exactamente 6 dígitos numéricos")]
        public required string Code { get; set; } 

        [Required(ErrorMessage = "Debes seleccionar el tipo de propiedad")]
        public required int PropertyTypeId { get; set; }

        [Required(ErrorMessage = "Debes seleccionar el tipo de venta")]
        public required int SaleTypeId { get; set; }

        [Required(ErrorMessage = "Debes ingresar el precio de la propiedad")]
        [Range(1, double.MaxValue, ErrorMessage = "El precio debe ser mayor que cero")]
        [DataType(DataType.Currency)]
        public required decimal Price { get; set; }

        [Required(ErrorMessage = "Debes ingresar la descripción de la propiedad")]
        [StringLength(2000, ErrorMessage = "La descripción no puede exceder los 2000 caracteres")]
        [DataType(DataType.MultilineText)]
        public required string Description { get; set; }

        [Required(ErrorMessage = "Debes ingresar el tamaño en metros")]
        [Range(1, int.MaxValue, ErrorMessage = "El tamaño debe ser mayor que cero")]
        public required int SizeInMeters { get; set; }

        [Required(ErrorMessage = "Debes ingresar la cantidad de habitaciones")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe tener al menos una habitación")]
        public required int Bedrooms { get; set; }

        [Required(ErrorMessage = "Debes ingresar la cantidad de baños")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe tener al menos un baño")]
        public required int Bathrooms { get; set; }

        [Required(ErrorMessage = "Debes seleccionar al menos una mejora")]
        [MinLength(1, ErrorMessage = "Debes seleccionar al menos una mejora")]
        public List<int> ImprovementsIds { get; set; } = new();

        [MaxLength(4, ErrorMessage = "No puedes subir más de 4 imágenes")]
        public List<IFormFile>? Images { get; set; } = new();
    }
}
