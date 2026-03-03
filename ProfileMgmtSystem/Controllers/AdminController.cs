using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProfileMgmtSystem.Models;
using ProfileMgmtSystem.Models.ViewModels;
using ProfileMgmtSystem.Services;

namespace ProfileMgmtSystem.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PersonService _personService;

        public AdminController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, PersonService personService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _personService = personService;
        }

        // GET: /Admin/Users
        public async Task<IActionResult> Users()
        {
            var users = _userManager.Users.ToList();
            var userRoles = new Dictionary<string, string>();

            foreach (var user in users)
            {
                var roles = await _userManager.GetRolesAsync(user);
                userRoles[user.Id] = roles.FirstOrDefault() ?? "No Role";
            }

            ViewBag.UserRoles = userRoles;
            return View(users);
        }

        // GET: /Admin/CreateUser
        [HttpGet]
        public IActionResult CreateUser()
        {
            ViewBag.Roles = new[] { "Admin", "User" };
            return View();
        }

        // POST: /Admin/CreateUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CreateUserViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Roles = new[] { "Admin", "User" };
                return View(model);
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, model.Role);
                TempData["Success"] = $"User {model.Email} created successfully with role {model.Role}!";
                return RedirectToAction(nameof(Users));
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError("", error.Description);

            ViewBag.Roles = new[] { "Admin", "User" };
            return View(model);
        }

        // POST: /Admin/DeleteUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return NotFound();

            // prevent admin from deleting themselves
            var currentUserId = _userManager.GetUserId(User);
            if (userId == currentUserId)
            {
                TempData["Error"] = "You cannot delete your own account!";
                return RedirectToAction(nameof(Users));
            }

            await _userManager.DeleteAsync(user);
            TempData["Success"] = "User deleted successfully!";
            return RedirectToAction(nameof(Users));
        }
    }
}