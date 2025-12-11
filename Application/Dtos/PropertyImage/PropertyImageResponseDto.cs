
namespace Application.Dtos.PropertyImage
{
    public class PropertyImageResponseDto
    {
        public required int Id { get; set; }
        public required int PropertyId { get; set; }
        public required string ImageUrl { get; set; }
    }
}