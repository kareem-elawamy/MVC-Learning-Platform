using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONE.Models
{
    public class FavoriteVideo
    {
        public int Id { get; set; }
        [Required]
        public string UserId { get; set; } 

        [ForeignKey("Video")]
        public int VideoId { get; set; }
        public Video Video { get; set; }
    }
}
