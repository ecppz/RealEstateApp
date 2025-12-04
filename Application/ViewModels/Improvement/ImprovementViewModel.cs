using Application.ViewModels.PropertyImprovement;

namespace Application.ViewModels.Improvement
{
    public class ImprovementViewModel
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public ICollection<PropertyImprovementViewModel>? PropertyImprovements { get; set; }
    }

}