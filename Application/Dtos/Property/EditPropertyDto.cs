using Microsoft.AspNetCore.Http;

namespace Application.Dtos.Property
{
    public class EditPropertyDto
    {
        public required int Id { get; set; }
        public required string Code { get; set; }
        public required int PropertyTypeId { get; set; }
        public required int SaleTypeId { get; set; }
        public required decimal Price { get; set; }
        public required string Description { get; set; }
        public required int SizeInMeters { get; set; }
        public required int Bedrooms { get; set; }
        public required int Bathrooms { get; set; }
        public List<int> ImprovementsIds { get; set; } = new();
        public List<IFormFile>? Images { get; set; } = new();
    }
}
