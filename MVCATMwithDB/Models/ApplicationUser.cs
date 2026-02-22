using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MVCATMwithDB.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Additional props besides inherited from identity
        [Required]
        [StringLength(100)]
        public string? FullName { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation property to link with Account
        public virtual Account? Account { get; set; }
    }
}