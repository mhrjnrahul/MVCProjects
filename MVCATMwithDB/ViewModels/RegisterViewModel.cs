using System.ComponentModel.DataAnnotations;

namespace MVCATMwithDB.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Account Number")]
        public string AccountNumber { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4)]
        [Display(Name = "PIN (4 digits)")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "PIN must be exactly 4 digits")]
        public string PIN { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "Initial Deposit")]
        public decimal InitialDeposit { get; set; } = 0m;
    }
}