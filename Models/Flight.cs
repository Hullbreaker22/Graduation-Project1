using SkyLine.ViewModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyLine.Models
{
    public class Flight
    {
        [Key]
        [Required]

        public int Flight_Id_PK { get; set; }

        [Required]
        public string Flight_Code { get; set; } = string.Empty;
        public string? Transate_FlightCode { get; set; }

        public double Duration { get; set; }
        public DateTime Leaving_Time { get; set; }
        public DateTime Arriving_Time { get; set; }

        [ForeignKey(nameof(LeavingAirport))]
        public int Leaving_Airport_ID { get; set; }

        [ForeignKey(nameof(ArriveAirport))]
        public int Arrive_Airport_ID { get; set; }

        [ForeignKey(nameof(AirLine))]
        public int AirLine_ID { get; set; }

        public decimal Price { get; set; }
        public int AvailableSeats { get; set; }
        public int StopPoints { get; set; }

        public ICollection<Fare>? Fares { get; set; }
        public AirPort? LeavingAirport { get; set; }
        public AirPort? ArriveAirport { get; set; }
        public Airline? AirLine { get; set; }

    }
}
