using Application.ViewModels.Improvement;
using Application.ViewModels.Property;

namespace Application.ViewModels.PropertyImprovement
{
    public class PropertyImprovementViewModel
    {
        public int PropertyId { get; set; }
        public int ImprovementId { get; set; }
        public ImprovementViewModel? Improvement { get; set; }
        public PropertyViewModel? Property { get; set; }
    }

}