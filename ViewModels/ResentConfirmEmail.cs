using System.ComponentModel.DataAnnotations;

namespace SkyLine.ViewModels
{
    public class ResentConfirmEmail
    {
        public int Id { get; set; }
        [Required]
        public string EmailOrUserName { get; set; } = string.Empty;
    }
}
