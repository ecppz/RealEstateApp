using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Offer
{
    public class ClientOffersViewModel
    {
        public required int ClientId { get; set; }
        public List<OfferViewModel> Offers { get; set; } = new();
    }

}
