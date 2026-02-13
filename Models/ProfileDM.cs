using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;



namespace SkyLine.Models
{
    public class ProfileDM : ApplicationUser
    {


        public string Name { get; set; } = string.Empty;
        //public string? Address { get; set; }
        public string? Street { get; set; }
        public string? Profile_IMG { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Image { get; set; }
        public string? ZipCode { get; set; }
        public int LoyaltyPoints { get; set; } = 0;

        public string Password { get; set; } = string.Empty;
        [Required, DataType(DataType.Password), Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = string.Empty;



    }
}
