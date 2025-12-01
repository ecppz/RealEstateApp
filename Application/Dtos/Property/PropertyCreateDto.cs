using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Property
{
    public class PropertyCreateDto
    {
        public required string Code { get; set; }
        public required int PropertyTypeId { get; set; }
        public required string AgentId { get; set; }
        public required int SaleTypeId { get; set; }
        public required decimal Price { get; set; }
        public required string Description { get; set; }
        public required int SizeInMeters { get; set; }
        public required int Bedrooms { get; set; }
        public required int Bathrooms { get; set; }
    }

}
