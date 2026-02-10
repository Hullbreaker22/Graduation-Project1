using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace SkyLine.Models
{
   
    public class Seat_Reservation
    {
        [Key]
        public int SeatId { get; set; }

        [ForeignKey(nameof(Flight))]
        public int FlightId { get; set; }
        public Flight Flight { get; set; }

        [Required]
        [MaxLength(10)]
        public string SeatCode { get; set; }

        public Cabin CabinClass { get; set; } // Enum: Economy, Business, etc.

        public bool IsOccupied { get; set; } = false;

        [ForeignKey(nameof(Passenger))]
        public int? OccupiedBy { get; set; }
        public Passenger Passenger { get; set; }

        [ForeignKey(nameof(Booking))]
        public int? BookingId { get; set; }
        public Booking Booking { get; set; }

        public double? Price { get; set; }
    }

}
