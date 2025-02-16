using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONE.Models
{
    public class Material
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, ForeignKey("Track")]
        public int TrackId { get; set; }
        public Track Track { get; set; }
        public ICollection<Video> Videos { get; set; } = new List<Video>();
    }
}
