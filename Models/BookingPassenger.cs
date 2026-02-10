using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyLine.Models
{
    public class BookingPassenger
    {

        [Key]
        public int Id_PK { get; set; }

        [ForeignKey(nameof(Booking))]
        public int Booking_Id_FK { get; set; }

        [ForeignKey(nameof(Passenger))]
        public int Passenger_Id_FK { get; set; }

        [ForeignKey(nameof(Fare))]
        public int Fare_Id { get; set; }

        public string? Seat_Num { get; set; }

        public Booking? Booking { get; set; }
        public Passenger? Passenger { get; set; }
        public Fare? Fare { get; set; }

    }
}
