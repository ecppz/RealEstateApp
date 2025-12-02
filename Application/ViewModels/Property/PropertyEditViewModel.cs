using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Property
{
    public class PropertyEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Code { get; set; } = string.Empty;

        [Required]
        public int PropertyTypeId { get; set; }

        [Required]
        public string AgentId { get; set; } = string.Empty;

        [Required]
        public int SaleTypeId { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public int SizeInMeters { get; set; }

        [Required]
        public int Bedrooms { get; set; }

        [Required]
        public int Bathrooms { get; set; }

        public PropertyStatus Status { get; set; }
    }
}
