using Application.ViewModels.Property;

namespace Application.ViewModels.PropertyImage
{
    public class PropertyImageViewModel
    {
        public required int Id { get; set; }
        public required int PropertyId { get; set; }
        public required string ImageUrl { get; set; }
        public PropertyViewModel? Property { get; set; }
    }


}