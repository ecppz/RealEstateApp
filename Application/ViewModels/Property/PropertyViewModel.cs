using Application.ViewModels.PropertyImage;
using Application.ViewModels.PropertyImprovement;
using Application.ViewModels.PropertyType;
using Application.ViewModels.SaleType;
using Domain.Common.Enums;

namespace Application.ViewModels.Property
{
    public class PropertyViewModel
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
        public PropertyTypeViewModel? PropertyType { get; set; }
        public SaleTypeViewModel? SaleType { get; set; }
        public ICollection<PropertyImageViewModel>? Images { get; set; }
        public ICollection<PropertyImprovementViewModel>? Improvements { get; set; }
    }

}
