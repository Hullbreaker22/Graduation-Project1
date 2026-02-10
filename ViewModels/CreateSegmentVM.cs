namespace SkyLine.ViewModels
{
    public class CreateSegmentVM
    {
        public int Flight_ID_Fk { get; set; }
        public string? Flight_Number { get; set; }
        public int Segment_Order { get; set; }
        public Cabin cabin { get; set; }
  
        public int Departure_Airport_ID_Fk { get; set; }

        public int Arrival_Airport_ID_Fk { get; set; }

        public DateTime Departure_Time { get; set; }
        public DateTime Arrival_Time { get; set; }

        public double Distance_KM { get; set; }
        public double Duration_Min { get; set; }

        public string? Aircraft_Type { get; set; }
        public double Layover_Duration_Min { get; set; }
        public double Distance_From_Origin { get; set; }
        public double Distance_To_Destination { get; set; }



    }
}
