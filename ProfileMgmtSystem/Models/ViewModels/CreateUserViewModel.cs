using System.ComponentModel.DataAnnotations;

namespace ProfileMgmtSystem.Models.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; } = string.Empty;

        [Required]
        public string Role { get; set; } = "User";
    }
}