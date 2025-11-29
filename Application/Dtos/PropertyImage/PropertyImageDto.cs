using Application.Dtos.Property;

namespace Application.Dtos.PropertyImage
{
    public class PropertyImageDto
    {
        public required int Id { get; set; }
        public required int PropertyId { get; set; }
        public required string ImageUrl { get; set; }
        public PropertyDto? Property { get; set; }
    }


}