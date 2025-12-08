using Application.ViewModels.PropertyImprovement;
using Domain.Common.Enums;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Application.ViewModels.Property
{
    public class CreatePropertyViewModel
    {
        public int Id { get; set; }

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
        public required ICollection<int> Improvements { get; set; } = new List<int>();

        [Required(ErrorMessage = "Debes subir al menos una imagen")]
        [MinLength(1, ErrorMessage = "Debes subir al menos una imagen")]
        [MaxLength(4, ErrorMessage = "No puedes subir más de 4 imágenes")]
        public ICollection<IFormFile>? Images { get; set; }
        public PropertyStatus Status { get; set; } = PropertyStatus.Available;
    }
}
