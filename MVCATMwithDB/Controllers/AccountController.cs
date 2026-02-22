using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MVCATMwithDB.Data;
using MVCATMwithDB.Models;
using MVCATMwithDB.ViewModels;

namespace MVCATMwithDB.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ATMDbContext _context;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ATMDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        // REGISTER - GET
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        // REGISTER - POST
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Check if account number already exists
                if (_context.Accounts.Any(a => a.AccountNumber == model.AccountNumber))
                {
                    ModelState.AddModelError("AccountNumber", "Account number already exists.");
                    return View(model);
                }

                // Create ApplicationUser
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FullName = model.FullName,
                    CreatedDate = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    // Create Account linked to this user
                    var account = new Account
                    {
                        AccountNumber = model.AccountNumber,
                        AccountHolderName = model.FullName,
                        PIN = model.PIN,
                        Balance = model.InitialDeposit,
                        IsActive = true,
                        CreatedDate = DateTime.Now,
                        UserId = user.Id
                    };

                    _context.Accounts.Add(account);
                    await _context.SaveChangesAsync();

                    // Log the initial deposit if any
                    if (model.InitialDeposit > 0)
                    {
                        var transaction = new Transaction
                        {
                            TransactionType = "Deposit",
                            Amount = model.InitialDeposit,
                            AccountId = account.AccountId,
                            Status = "Success",
                            BalanceBefore = 0,
                            BalanceAfter = model.InitialDeposit,
                            TransactionDate = DateTime.Now
                        };
                        _context.Transactions.Add(transaction);
                        await _context.SaveChangesAsync();
                    }

                    // Sign in the user
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Dashboard", "ATM");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        // LOGIN - GET
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        // LOGIN - POST
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("Dashboard", "ATM");
                    }
                }
                if (result.IsLockedOut)
                {
                    ViewBag.Error = "Account is locked out. Please try again later.";
                    return View(model);
                }
                else
                {
                    ViewBag.Error = "Invalid login attempt.";
                    return View(model);
                }
            }

            return View(model);
        }

        // LOGOUT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        // ACCESS DENIED
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}