using Application.Dtos.Property;

namespace Application.Dtos.Message
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public required string SenderId { get; set; } 
        public required string ReceiverId { get; set; } 
        public required string Content { get; set; }
        public DateTime SentAt { get; set; } = DateTime.Now;
        public PropertyDto? Property { get; set; }
    }

}