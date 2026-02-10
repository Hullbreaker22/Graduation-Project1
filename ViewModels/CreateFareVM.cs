namespace SkyLine.ViewModels
{
    public class CreateFareVM
    {
        public int flightId { get; set; }
        public Cabin CabinClass { get; set; }
        public double price { get; set; }
        public bool Refundable { get; set; }
        public bool Changeable { get; set; }
        public string Baggage_Allowance { get; set; }
        public bool Seat_Selection { get; set; }

    }
}
