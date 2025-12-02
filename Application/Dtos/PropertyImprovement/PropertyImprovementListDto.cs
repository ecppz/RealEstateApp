using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.PropertyImprovement
{
    public class PropertyImprovementListDto
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public int ImprovementId { get; set; }

        public string? PropertyCode { get; set; }
        public string? ImprovementName { get; set; }
    }
}
