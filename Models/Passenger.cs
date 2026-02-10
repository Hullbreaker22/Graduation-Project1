using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SkyLine.Models
{

    public enum Passenger_Type
    {
        Adult,
        Child,
        Infant,
        Senior,
        Student,
        Military
    
      
    }


    public class Passenger
    {
        [Key]
        [Required]

        public int Passenger_Id { get; set; }

        [ForeignKey(nameof(User))]
        public string? User_Id { get; set; }
        public string SeatCode { get; set; }


        [Required]
        public string Full_Name { get; set; } = string.Empty;

        public string Passport_Num { get; set; } = string.Empty;
        public string Nationality { get; set; } = string.Empty;
        public DateTime DOB { get; set; }
        public Passenger_Type Passenger_Type { get; set; } 

        public ApplicationUser? User { get; set; }
    }
}
