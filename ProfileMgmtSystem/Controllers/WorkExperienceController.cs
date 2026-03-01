using Microsoft.AspNetCore.Mvc;
using ProfileMgmtSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using ProfileMgmtSystem.Models;

namespace ProfileMgmtSystem.Controllers
{
    [Authorize]
    public class WorkExperienceController : Controller
    {
        //store the service first
        private readonly WorkExperienceService _service;
        private readonly PersonService _personService;
        private readonly UserManager<ApplicationUser> _userManager;

        //initialise the constructor to inject the service
        public WorkExperienceController(WorkExperienceService service, PersonService personService, UserManager<ApplicationUser> userManager)
        {
            _service = service;
            _personService = personService;
            _userManager = userManager;
        }

        //create work experience for a person by their id
        //first of all get the view to create work experience for a person by their id
        [HttpGet]
        public IActionResult Create(int personId)
        {
            ViewBag.PersonId = personId;
            return View();
        }

        //post method to actually create the work experience for a person by their id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int personId, string companyName, string position, int startYear, int endYear)
        {
            if(!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                var person = await _personService.GetByUserIdAsync(userId!);
                if(person == null || person.Id != personId)
                {
                    return Forbid(); //403 forbidden if they try to add work experience to someone else's profile
                }
            }
            await _service.CreateAsync(personId, companyName, position, startYear, endYear);
            return RedirectToAction("Details", "Person", new { id = personId });
        }

        //edit the work experience for a person by their id
        //first of all get the view to edit the work experience for a person by their id
        [HttpGet]
        [Route("WorkExperience/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var experience = await _service.GetByIdAsync(id);
            if (experience == null)
            {
                return NotFound();
            }

            if(!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                var person = await _personService.GetByIdAsync(experience.PersonId);
                if(person == null || person.UserId != userId)
                {
                    return Forbid(); //403 forbidden if they try to edit work experience of someone else's profile
                }
            }
            return View(experience);
        }

        //form to actually edit the work experience for a person by their id
        [HttpPost]
        [Route("WorkExperience/Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int personId, string companyName, string position, int startYear, int endYear)
        {
            var skill = await _service.GetByIdAsync(id);
            if (skill == null)
            {
                return NotFound();
            }

            if(!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                var person = await _personService.GetByIdAsync(personId);
                if(person == null || person.UserId != userId)
                {
                    return Forbid(); //403 forbidden if they try to edit work experience of someone else's profile
                }
            }
            await _service.UpdateAsync(id, companyName, position, startYear, endYear);
            return RedirectToAction("Details", "Person", new { id = personId });
        }

        //delete the work experience for a person by their id
        //first of all get the view to delete the work experience for a person by their id
        [HttpGet]
        [Route("WorkExperience/Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var experience = await _service.GetByIdAsync(id);
            if (experience == null)
            {
                return NotFound();
            }

            if(!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                var person = await _personService.GetByIdAsync(experience.PersonId);
                if(person == null || person.UserId != userId)
                {
                    return Forbid(); //403 forbidden if they try to delete work experience of someone else's profile
                }
            }
            return View(experience);
        }

        //form to actually delete the work experience for a person by their id
        [HttpPost]
        [Route("WorkExperience/Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int personId)
        {
            var experience = await _service.GetByIdAsync(id);
            if (experience == null)
            {
                return NotFound();
            }

            if(!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                var person = await _personService.GetByIdAsync(experience.PersonId);
                if(person == null || person.UserId != userId)
                {
                    return Forbid(); //403 forbidden if they try to delete work experience of someone else's profile
                }
            }
            return RedirectToAction("Details", "Person", new { id = personId });
        }
    }
}
