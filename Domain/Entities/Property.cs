using Domain.Common.Enums;

namespace Domain.Entities
{
    public class Property
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
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        //nav property
        public PropertyType? PropertyType { get; set; }
        public SaleType? SaleType { get; set; }
        public ICollection<PropertyImage>? Images { get; set; }
        public ICollection<PropertyImprovement>? Improvements { get; set; }
    }

}
