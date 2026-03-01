using Microsoft.AspNetCore.Mvc;
using ProfileMgmtSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProfileMgmtSystem.Models;
using System.Threading.Tasks;

namespace ProfileMgmtSystem.Controllers
{
    [Authorize] // Ensure that only authenticated users can access these actions
    public class EducationController : Controller
    {
        //store the service in a private readonly field
        private readonly EducationService _educationService;

        //store the person service to check if the person belongs to the current user in the create method, so we inject it into the controller
        private readonly PersonService _personService;

        //store user manager to get the current user's id and check their role in the details method, so we inject it into the controller
        private readonly UserManager<ApplicationUser> _userManager;

        //initialise the constructor to inject the service
        public EducationController(EducationService educationService,PersonService personService, UserManager<ApplicationUser> userManager)
        {
            _educationService = educationService;
            _personService = personService;
            _userManager = userManager;
        }

        //get education by id
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var education = await _educationService.GetPersonAsync(id);
            if (education == null) return NotFound();
            return View(education);
        }

        //create education for a person
        [HttpGet]
        public async Task<IActionResult> Create(int personId)
        {
            //if not admin, they can only add to their own profile, so we need to check if the person belongs to the current user
            if(!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                var person = await _personService.GetByUserIdAsync(userId!);
                if(person == null || person.Id != personId)
                {
                    return Forbid(); //403 forbidden if they try to add education to someone else's profile
                }
            }
            ViewBag.PersonId = personId;  // store it so the view can use it
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int personId, string institutionName, string degree, string fieldOfStudy, int startYear, int endYear)
        {
            //check whether admin or not
            if (!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                var person = await _personService.GetByUserIdAsync(userId!);
                if (person == null || person.Id != personId)
                {
                    return Forbid(); //403 forbidden if they try to add education to someone else's profile
                }
            }

                await _educationService.CreateAsync(personId, institutionName, degree, fieldOfStudy, startYear, endYear);
                return RedirectToAction("Details", "Person", new { id = personId });

        }

        //edit education by id
        [HttpGet]
        [Route("Education/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var education = await _educationService.GetByIdAsync(id);
            if (education == null) return NotFound();

            //check admin or not
            if(!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                var person = await _personService.GetByUserIdAsync(userId!);
                if(person == null || person.Id != education.PersonId)
                {
                    return Forbid(); //403 forbidden if they try to edit education of someone else's profile
                }
            }   
            return View(education);
        }

        //here form is submitted and we update the education
        //we get the education by id and then update the education
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Education/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, int personId, string institutionName, string degree, string fieldOfStudy, int startYear, int endYear)
        {
            var education = await _educationService.GetByIdAsync(id);
            if (education == null) return NotFound();

            //check admin or not
            if(!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                var person = await _personService.GetByUserIdAsync(userId!);
                if(person == null || person.Id != education.PersonId)
                {
                    return Forbid(); //403 forbidden if they try to edit education of someone else's profile
                }
            }   
             await _educationService.UpdateAsync(id, institutionName, degree, fieldOfStudy, startYear, endYear);
             return RedirectToAction("Details", "Person", new { id = personId });
        }

        //delete education by
        [HttpGet]
        [Route("Education/Delete/{id:int}")]

        public async Task<IActionResult> Delete(int id)
        {
            var education = await _educationService.GetByIdAsync(id);
            if (education == null) return NotFound();

            if(!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                var person = await _personService.GetByUserIdAsync(userId!);
                if(person == null || person.Id != education.PersonId)
                {
                    return Forbid(); //403 forbidden if they try to delete education of someone else's profile
                }
            }
            return View(education);
        }

        [HttpPost]
        [Route("Education/Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int personId)
        {
            var education = await _educationService.GetByIdAsync(id);
            if (education == null) return NotFound();

            if(!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                var person = await _personService.GetByUserIdAsync(userId!);
                if(person == null || person.Id != education.PersonId)
                {
                    return Forbid(); //403 forbidden if they try to delete education of someone else's profile
                }
            }   
             await _educationService.DeleteAsync(id);
             return RedirectToAction("Details", "Person", new { id = personId });
        }
     }
}
