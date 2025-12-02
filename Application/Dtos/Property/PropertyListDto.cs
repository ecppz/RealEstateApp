using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Property
{
    public class PropertyListDto
    {
        public required int Id { get; set; }
        public required string Code { get; set; }
        public required string PropertyTypeName { get; set; }
        public required string SaleTypeName { get; set; }
        public required decimal Price { get; set; }
        public required int Bedrooms { get; set; }
        public required int Bathrooms { get; set; }
        public required int SizeInMeters { get; set; }
        public PropertyStatus Status { get; set; }
    }

}
