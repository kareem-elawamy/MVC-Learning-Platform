using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ONE.Models
{
    public class Book
    {

        [Key]
        public int Id { get; set; }
        [Required]
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        [Required]
        public string? fillName { get; set; }

        [ForeignKey("Material")]
        public int MaterialID { get; set; }
        public Material? Material { get; set; }
    }
}
