
using Application.ViewModels.Property;

namespace Application.ViewModels.Favorite
{
    public class FavoriteViewModel
    {
        public required int Id { get; set; }
        public required string ClientId { get; set; }
        public required int PropertyId { get; set; }
        public PropertyViewModel? Properties { get; set; }
    }
}