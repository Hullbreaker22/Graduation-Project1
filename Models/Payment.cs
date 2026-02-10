using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyLine.Models
{
    public enum Payment_status
    {
        Pending,
        Completed,
        Cancelled
    }

    public enum Payment_Method
    {
        Visa,
        Credit,
        Cash
    }


    public class Payment
    {
        [Key]
        public int Payment_Id_PK { get; set; }

        [ForeignKey(nameof(Booking))]
        public int Booking_Id_FK { get; set; }

        public decimal Amount { get; set; }
        public Payment_status Status { get; set; } 
        public string? Payment_Ref_Number { get; set; }
        public DateTime Created_Date { get; set; } = DateTime.UtcNow;
        public Payment_Method PaymentMethod { get; set; } 

        public Booking? Booking { get; set; }
    }
}
