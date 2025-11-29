using Application.Dtos.PropertyImage;
using Application.Dtos.PropertyImprovement;
using Application.Dtos.PropertyType;
using Application.Dtos.SaleType;
using Domain.Common.Enums;

namespace Application.Dtos.Property
{
    public class PropertyDto
    {
        public required int Id { get; set; }
        public required string Code { get; set; } 
        public required int PropertyTypeId { get; set; }
        public required string AgentId { get; set; } 
        public required int SaleTypeId { get; set; }
        public required decimal Price { get; set; }
        public required string Description { get; set; }
        public required int SizeInMeters { get; set; }
        public required int Bedrooms { get; set; }
        public required int Bathrooms { get; set; }
        public PropertyStatus Status { get; set; }
        
        //nav property
        public PropertyTypeDto? PropertyType { get; set; }
        public SaleTypeDto? SaleType { get; set; }
        public ICollection<PropertyImageDto>? Images { get; set; }
        public ICollection<PropertyImprovementDto>? Improvements { get; set; }
    }

}
