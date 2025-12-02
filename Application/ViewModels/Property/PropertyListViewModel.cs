using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Property
{
    public class PropertyListViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string TypeName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Bedrooms { get; set; }
        public int Bathrooms { get; set; }
        public int SizeInMeters { get; set; }
        public PropertyStatus Status { get; set; }
    }
}
