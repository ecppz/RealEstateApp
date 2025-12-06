using Application.Dtos.Property;
using Domain.Common.Enums;

namespace Application.Dtos.Offer
{
    public class OfferDto
    {
        public required int Id { get; set; }
        public required int PropertyId { get; set; }
        public required string ClientId { get; set; } 
        public required decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public OfferStatus Status { get; set; }
        public PropertyDto? Property { get; set; }
    }
}