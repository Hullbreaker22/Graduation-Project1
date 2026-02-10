using System.ComponentModel.DataAnnotations;

namespace SkyLine.ViewModels
{
    public class EditFlightVM
    {

        public int Flight_Id_PK { get; set; }

        public string Flight_Code { get; set; } = string.Empty;
        public string? Transate_FlightCode { get; set; }
        public double Duration { get; set; }
        public DateTime Leaving_Time { get; set; }
        public DateTime Arriving_Time { get; set; }
      
        public int Leaving_Airport_ID { get; set; }

        public int Arrive_Airport_ID { get; set; }

        public int AirLine_ID { get; set; }

        public decimal Price { get; set; }
        public int AvailableSeats { get; set; }
        public int StopPoints { get; set; }
  

    }
}
