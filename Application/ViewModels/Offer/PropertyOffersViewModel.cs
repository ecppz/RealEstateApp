using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Offer
{
    public class PropertyOffersViewModel
    {
        public required int PropertyId { get; set; }
        public List<OfferViewModel> Offers { get; set; } = new();
    }

}
