using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MVCATMwithDB.Data;
using MVCATMwithDB.Models;

namespace MVCATMwithDB.Controllers
{
    [Authorize]  // Require authentication for all actions in this controller
    public class ATMController : Controller
    {
        private readonly ATMDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ATMController(ATMDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // HELPER METHOD - Get Current Logged-in Account
        private async Task<Account?> GetCurrentAccountAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return null;

            return await _context.Accounts
                .Include(a => a.Transactions)
                .FirstOrDefaultAsync(a => a.UserId == user.Id);
        }

        // Create Transaction Log
        private async Task LogTransactionAsync(Account account, string type, decimal? amount, bool success, string? failureReason = null)
        {
            var balanceBefore = account.Balance;

            // Update balance if successful
            if (success && amount.HasValue)
            {
                if (type == "Withdrawal")
                    account.Balance -= amount.Value;
                else if (type == "Deposit")
                    account.Balance += amount.Value;
            }

            // Create transaction record
            var transaction = new Transaction
            {
                TransactionType = type,
                Amount = amount,
                AccountId = account.AccountId,
                Status = success ? "Success" : "Failed",
                FailureReason = failureReason,
                BalanceBefore = balanceBefore,
                BalanceAfter = account.Balance,
                TransactionDate = DateTime.Now
            };

            _context.Transactions.Add(transaction);
            await _context.SaveChangesAsync();
        }

        // DASHBOARD
        public async Task<IActionResult> Dashboard()
        {
            var account = await GetCurrentAccountAsync();
            if (account == null)
            {
                return RedirectToAction("Login", "Account");
            }

            return View(account);
        }

        // CHECK BALANCE
        public async Task<IActionResult> Balance()
        {
            var account = await GetCurrentAccountAsync();
            if (account == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Log balance inquiry
            await LogTransactionAsync(account, "BalanceInquiry", null, true);

            return View(account);
        }

        // WITHDRAW - GET
        [HttpGet]
        public IActionResult Withdraw()
        {
            return View();
        }

        // WITHDRAW - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Withdraw(decimal amount)
        {
            var account = await GetCurrentAccountAsync();
            if (account == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Validate amount
            if (amount <= 0)
            {
                ViewBag.Error = "Please enter a valid amount!";
                return View();
            }

            // Check sufficient funds
            if (amount > account.Balance)
            {
                ViewBag.Error = "Insufficient funds!";
                await LogTransactionAsync(account, "Withdrawal", amount, false, "Insufficient funds");
                return View();
            }

            // Process withdrawal
            await LogTransactionAsync(account, "Withdrawal", amount, true);
            ViewBag.Success = $"Successfully withdrew ${amount:N2}. New balance: ${account.Balance:N2}";

            return View();
        }

        // DEPOSIT - GET
        [HttpGet]
        public IActionResult Deposit()
        {
            return View();
        }

        // DEPOSIT - POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deposit(decimal amount)
        {
            var account = await GetCurrentAccountAsync();
            if (account == null)
            {
                return RedirectToAction("Login", "Account");
            }

            // Validate amount
            if (amount <= 0)
            {
                ViewBag.Error = "Please enter a valid amount!";
                return View();
            }

            // Process deposit
            await LogTransactionAsync(account, "Deposit", amount, true);
            ViewBag.Success = $"Successfully deposited ${amount:N2}. New balance: ${account.Balance:N2}";

            return View();
        }

        // MINI STATEMENT
        public async Task<IActionResult> MiniStatement()
        {
            var account = await GetCurrentAccountAsync();
            if (account == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var transactions = await _context.Transactions
                .Where(t => t.AccountId == account.AccountId)
                .OrderByDescending(t => t.TransactionDate)
                .Take(10)
                .ToListAsync();

            return View(transactions);
        }
    }
} 