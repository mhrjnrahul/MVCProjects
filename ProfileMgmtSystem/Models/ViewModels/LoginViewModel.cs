using System.ComponentModel.DataAnnotations;

namespace ProfileMgmtSystem.Models.ViewModels
{
    public class LoginViewModel
    {
        //we include fields only necessary for the login process, such as email and password
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }
    }
}
