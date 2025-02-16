using System.ComponentModel.DataAnnotations;

namespace ONE.Models
{
    public class ChatMessage
    {
        [Key]
        public int Id { get; set; } 

        [Required]
        public string UserMessage { get; set; }

        public string BotResponse { get; set; }

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
        [Required]
        public string UserId { get; set; }
    }

}
