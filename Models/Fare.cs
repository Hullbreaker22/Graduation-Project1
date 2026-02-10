using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyLine.Models
{

    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Fare
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey(nameof(flight))]
        public int flightId { get; set; }
        public Flight? flight { get; set; }

        public Cabin CabinClass { get; set; }

        public double Price { get; set; }       
        public double Taxes { get; set; }  
        public double Fees { get; set; }        

        public bool Refundable { get; set; }    
        public double RefundFee { get; set; }  

        public bool Changeable { get; set; }   
        public double ChangeFee { get; set; }   

        public string Baggage_Allowance { get; set; } 
        public bool Seat_Selection { get; set; }      

    }

}
