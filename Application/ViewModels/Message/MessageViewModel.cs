using Application.ViewModels.Property;

namespace Application.ViewModels.Message
{
    public class MessageViewModel
    {
        public int Id { get; set; }
        public int PropertyId { get; set; }
        public required string SenderId { get; set; } 
        public required string ReceiverId { get; set; } 
        public required string Content { get; set; }
        public DateTime SentAt { get; set; }
        public PropertyViewModel? Property { get; set; }
    }

}