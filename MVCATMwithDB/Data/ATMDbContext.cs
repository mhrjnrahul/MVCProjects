using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MVCATMwithDB.Models;

namespace MVCATMwithDB.Data
{
    // Change from DbContext to IdentityDbContext<ApplicationUser>
    public class ATMDbContext : IdentityDbContext<ApplicationUser>
    {
        public ATMDbContext(DbContextOptions<ATMDbContext> options)
            : base(options)
        {
        }

        // DbSets - EF will create tables from these
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        // Configure database schema
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);  // IMPORTANT: Call base first for Identity tables

            // Account configuration
            modelBuilder.Entity<Account>(entity =>
            {
                // Primary key starts at 1001
                entity.Property(e => e.AccountId)
                    .UseIdentityColumn(1001, 1);

                // Unique constraint on AccountNumber
                entity.HasIndex(e => e.AccountNumber)
                    .IsUnique();

                // Configure Balance precision properly
                entity.Property(e => e.Balance)
                    .HasPrecision(18, 2);

                // Check constraint for Balance
                entity.ToTable(t => t.HasCheckConstraint("CK_Account_Balance", "[Balance] >= 0"));

                // One-to-many relationship with Transactions
                entity.HasMany(a => a.Transactions)
                    .WithOne(t => t.Account)
                    .HasForeignKey(t => t.AccountId)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relationship with ApplicationUser
                entity.HasOne(a => a.User)
                    .WithOne(u => u.Account)
                    .HasForeignKey<Account>(a => a.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Transaction configuration
            modelBuilder.Entity<Transaction>(entity =>
            {
                // Primary key starts at 10001
                entity.Property(e => e.TransactionId)
                    .UseIdentityColumn(10001, 1);

                // Configure decimal fields with precision
                entity.Property(e => e.Amount)
                    .HasPrecision(18, 2);

                entity.Property(e => e.BalanceBefore)
                    .HasPrecision(18, 2);

                entity.Property(e => e.BalanceAfter)
                    .HasPrecision(18, 2);

                // Check constraints
                entity.ToTable(t =>
                {
                    t.HasCheckConstraint("CK_Transaction_Type",
                        "[TransactionType] IN ('Withdrawal', 'Deposit', 'BalanceInquiry')");
                    t.HasCheckConstraint("CK_Transaction_Status",
                        "[Status] IN ('Success', 'Failed')");
                });
            });
        }
    }
}