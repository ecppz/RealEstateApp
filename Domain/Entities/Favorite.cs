namespace Domain.Entities
{
    public class Favorite
    {
        public required int Id { get; set; }
        public required string ClientId { get; set; }
        public required int PropertyId { get; set; }
        public Property? Properties { get; set; }
    }
}