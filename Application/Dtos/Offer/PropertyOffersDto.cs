using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.Offer
{
    public class PropertyOffersDto
    {
        public required int PropertyId { get; set; }
        public List<OfferDto> Offers { get; set; } = new();
    }

}
