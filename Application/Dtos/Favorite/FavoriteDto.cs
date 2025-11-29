using Application.Dtos.Property;

namespace Application.Dtos.Favorite
{
    public class FavoriteDto
    {
        public required int Id { get; set; }
        public required string ClientId { get; set; }
        public required int PropertyId { get; set; }
        public PropertyDto? Properties { get; set; }
    }
}