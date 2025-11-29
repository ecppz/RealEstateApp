using Application.Dtos.Improvement;
using Application.Dtos.Property;

namespace Application.Dtos.PropertyImprovement
{
    public class PropertyImprovementDto
    {
        public int PropertyId { get; set; }
        public int ImprovementId { get; set; }
        public ImprovementDto? Improvement { get; set; }
        public PropertyDto? Property { get; set; }
    }

}