using System.ComponentModel.DataAnnotations;

namespace ONE.Models
{
    public class Track
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Material> Materials { get; set; } = new List<Material>();

    }
}
