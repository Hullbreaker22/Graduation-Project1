namespace SkyLine.ViewModels
{
    public class BookingRequest
    {

        public int PassengerCount { get; set; }
        public List<Passenger> Passengers { get; set; }
        public string SelectedSeats { get; set; }  // Example: "A1,B3,C5"
        public decimal TotalPrice { get; set; }
        public int Flight_Id_PK { get; set; }
        public int Fare_ID { get; set; }
    }
}
