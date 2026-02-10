using System.ComponentModel.DataAnnotations;

namespace SkyLine.ViewModels
{
    public class UserLogin
    {
        public string EmailOrUserName { get; set; } = string.Empty;
        [Required, DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}
