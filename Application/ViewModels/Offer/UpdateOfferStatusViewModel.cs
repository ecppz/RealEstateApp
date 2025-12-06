using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.ViewModels.Offer
{
    public class UpdateOfferStatusViewModel
    {
        public required int OfferId { get; set; }
        public required OfferStatus Status { get; set; }
    }

}
