using System.ComponentModel.DataAnnotations;

namespace SkyLine.Models
{
    public class City
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
    }
}
