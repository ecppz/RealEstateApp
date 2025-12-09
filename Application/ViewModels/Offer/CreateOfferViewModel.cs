using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Offer
{
    public class CreateOfferViewModel
    {
        public required int PropertyId { get; set; }
        public required decimal Amount { get; set; }
    }

}
