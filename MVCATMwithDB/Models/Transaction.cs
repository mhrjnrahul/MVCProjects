using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCATMwithDB.Models
{
    public class Transaction
    {
        [Key]
        public long TransactionId { get; set; }

        [Required]
        [StringLength(50)]
        public string TransactionType { get; set; }  // Withdrawal, Deposit, BalanceInquiry

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }  // Nullable for balance inquiry

        // Foreign Key
        [Required]
        public int AccountId { get; set; }

        [Required]
        [StringLength(20)]
        public string Status { get; set; }  // Success, Failed

        [StringLength(200)]
        public string? FailureReason { get; set; }  // Nullable!

        [Column(TypeName = "decimal(18,2)")]
        public decimal? BalanceBefore { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? BalanceAfter { get; set; }

        public DateTime TransactionDate { get; set; } = DateTime.Now;

        // Navigation Property - Transaction belongs to Account
        [ForeignKey("AccountId")]
        public virtual Account Account { get; set; }
    }
}