using Microsoft.AspNetCore.Mvc;
using ProfileMgmtSystem.Services;
using Microsoft.AspNetCore.Authorization;
using ProfileMgmtSystem.Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ProfileMgmtSystem.Controllers
{
    [Authorize]
    public class PersonController : Controller
    {
        
        private readonly PersonService _personService;
        //we need the user manager to get the current user's id and check their role in the details method, so we inject it into the controller
        private readonly UserManager<ApplicationUser> _userManager;

        public PersonController(PersonService personService, UserManager<ApplicationUser> userManager)
        {
            _personService = personService;
            _userManager = userManager;
        }

        //get all person
        //get the persons and then return them in the view
        [Authorize(Roles = "Admin")] //only admin can see the list of all persons
        public async Task<IActionResult> Index()
        {
            var persons = await _personService.ListtAllAsync();
            return View(persons);
        }

        // GET: /Person/MyProfile
        public async Task<IActionResult> MyProfile()
        {
            var userId = _userManager.GetUserId(User);
            var person = await _personService.GetByUserIdAsync(userId!);
            if (person == null)
                return RedirectToAction(nameof(Create)); // no profile yet
            return RedirectToAction(nameof(Portfolio));
        }

        // GET: /Person/Portfolio
        public async Task<IActionResult> Portfolio()
        {
            var userId = _userManager.GetUserId(User);
            var person = await _personService.GetByUserIdAsync(userId!);
            if (person == null)
                return RedirectToAction(nameof(Create));
            return View(person);
        }

        //get a person detail by id
        //details.cshtml
        public async Task<IActionResult> Details(int id)
        {
            var person = await _personService.GetByIdAsync(id);
            if (person == null) return NotFound();

            //if user role, they can only see their own profile, so we need to check if the person belongs to the current user
            if(!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                if(person.UserId != userId)
                {
                    return Forbid(); //403 forbidden if they try to access someone else's profile
                }
            }

            return View(person);
        }

        //create person
        //create.cshtml
        //get method
        //form auxa
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        //post method
        //both roles can create a profile, but a user can only create a profile for themselves,
        //so we need to get the current user's id and pass it to the service when creating a new person
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            var userId = _userManager.GetUserId(User);
            await _personService.CreateAsync(firstName, lastName, email, dateOfBirth, userId);

            if(User.IsInRole("Admin"))
            {
                return RedirectToAction(nameof(Index)); //admin goes to the list of all persons after creating a new person
            }
            else
            {
                var person = await _personService.GetByUserIdAsync(userId!); //get the newly created person by user id
                return RedirectToAction(nameof(Details), new { id = person!.Id }); //user goes to their own profile after creating it
            }

        }

        //edit method
        //edit by id of person
        //u get the person by id and then return the person in the view to edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var person = await _personService.GetByIdAsync(id);
            if (person == null) return NotFound();

            if(User.IsInRole("Admin"))
            {
                return View(person); //admin can edit any profile
            }
            else
            {
                var userId = _userManager.GetUserId(User);
                if(person.UserId != userId)
                {
                    return Forbid(); //403 forbidden if they try to edit someone else's profile
                }
            }
            return View(person);
        }

        //post method for edit
        //here u get the form data and then update the person in the database
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string? email = null, string? firstName = null)
        {
            var person = await _personService.GetByIdAsync(id);
            if (person == null) return NotFound();

            if (!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                if (person.UserId != userId) return Forbid();
            }

            await _personService.UpdateAsync(id, email, firstName);
            return RedirectToAction(nameof(Details), new { id });
        }

        //if (!ModelState.IsValid) return View();
        //var success = await _personService.UpdateAsync(id, email, firstName);
        //if (!success) return NotFound();
        ////nameof is a way to get the name of a method as a string, which is useful for refactoring and avoiding hard-coded strings
        ////if we change the name of the Index method, this will automatically update the string in RedirectToAction, preventing errors and improving maintainability
        //return RedirectToAction(nameof(Index));
        //}

        //delete method
        //get the person by id and then return the person in the view to confirm deletion
        //admin only can delete a profile, so we check the role before allowing access to the delete view
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var person = await _personService.GetByIdAsync(id);
            if (person == null) return NotFound();

            return View(person);
        }

        //post method for delete
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //action name is used to specify the name of the action method that will handle the form submission, which is useful when the method name does not match the action name in the view
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _personService.DeleteAsync(id);
            if (!success) return NotFound();
            return RedirectToAction(nameof(Index));

        }
    }
}