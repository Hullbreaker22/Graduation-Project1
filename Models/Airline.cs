using System.ComponentModel.DataAnnotations;

namespace SkyLine.Models
{
    public class Airline
    {
        [Required]
        [Key]
        public int Id  { get; set; }
        [Required]
        public string  Code { get; set; }
        public string?  logo { get; set; }
        [Required]
        public string  Name { get; set; }
        public string?  Description { get; set; }

    }
}
