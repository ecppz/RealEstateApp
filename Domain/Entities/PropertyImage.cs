namespace Domain.Entities
{
    public class PropertyImage
    {
        public required int Id { get; set; }
        public required int PropertyId { get; set; }
        public required string ImageUrl { get; set; }
        public Property? Property { get; set; }
    }


}