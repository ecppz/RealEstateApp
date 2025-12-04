using Application.Dtos.PropertyImage;
using Domain.Common.Enums;
namespace Application.Dtos.Property
{
    public class CreatePropertyDto
    {
        public required int Id { get; set; }
        public required string Code { get; set; }
        public required int PropertyTypeId { get; set; }
        public required int SaleTypeId { get; set; }
        public required string AgentId { get; set; }
        public required  decimal Price { get; set; }
        public required  string Description { get; set; }
        public required int SizeInMeters { get; set; }
        public required int Bedrooms { get; set; }
        public required int Bathrooms { get; set; }
        public PropertyStatus Status { get; set; } = PropertyStatus.Available;
        public required List<int> ImprovementsIds { get; set; } = new();
        public List<PropertyImageDto> Images { get; set; } = new();
    }
}
