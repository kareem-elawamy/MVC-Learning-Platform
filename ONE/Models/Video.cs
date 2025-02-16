using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONE.Models
{
    public class Video
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string VideoId { get; set; }
        
        public string? Title { get; set; }
        public string? ThumbnailUrl { get; set; } 
        public string? Description { get; set; } 
        [ ForeignKey("Material")]
        public int MaterialID { get; set; }
        public Material? Material { get; set; }
    }
}
