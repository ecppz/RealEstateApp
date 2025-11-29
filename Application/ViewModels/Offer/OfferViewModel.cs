using Application.ViewModels.Property;
using Domain.Common.Enums;

namespace Application.ViewModels.Offer
{
    public class OfferViewModel
    {
        public required int Id { get; set; }
        public required int PropertyId { get; set; }
        public required int ClientId { get; set; } 
        public required decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public OfferStatus Status { get; set; }
        public PropertyViewModel? Property { get; set; }
    }
}