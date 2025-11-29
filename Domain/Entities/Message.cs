namespace Domain.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public required string SenderId { get; set; } 
        public required string ReceiverId { get; set; } 
        public required string Content { get; set; }
        public DateTime SentAt { get; set; }
        public Property? Property { get; set; }
    }

}