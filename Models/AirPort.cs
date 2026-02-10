using System.ComponentModel.DataAnnotations;

namespace SkyLine.Models
{
    public class AirPort
    {
        [Required]
        [Key]
        public int Id  { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public int cityId { get; set; }
        public int terminal { get; set; }
        public City city { get; set; }

        public ICollection<Flight>? DepartingFlights { get; set; }
        public ICollection<Flight>? ArrivingFlights { get; set; }

        public ICollection<FlightSegment>? DepartingSegments { get; set; }
        public ICollection<FlightSegment>? ArrivingSegments { get; set; }
    }
}
