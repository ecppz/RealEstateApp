using Domain.Common.Enums;

namespace Domain.Entities
{
    public class Offer
    {
        public int Id { get; set; }
        public required int PropertyId { get; set; }
        public required string ClientId { get; set; } 
        public required decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public OfferStatus Status { get; set; }
        public Property? Property { get; set; }
    }
}