using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCATMwithDB.Models
{
    public class Account
    {
        [Key]
        public int AccountId { get; set; }

        // Properties with validation
        [Required(ErrorMessage = "Account number is required")]
        [StringLength(20)]
        public string AccountNumber { get; set; }

        [Required(ErrorMessage = "Account holder name is required")]
        [StringLength(100)]
        public string AccountHolderName { get; set; }

        [Required(ErrorMessage = "PIN is required")]
        [StringLength(4, MinimumLength = 4, ErrorMessage = "PIN must be exactly 4 digits")]
        public string PIN { get; set; }  

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue, ErrorMessage = "Balance cannot be negative")]
        public decimal Balance { get; set; } = 0.00m;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        //for identity
        public string? UserId { get; set; } //fk for application user

        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }//navigation prop

        // Navigation property - One Account has Many Transactions
        public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}