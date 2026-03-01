using Microsoft.AspNetCore.Mvc;
using ProfileMgmtSystem.Services;
using ProfileMgmtSystem.Models;

namespace ProfileMgmtSystem.Controllers
{
    public class EducationController : Controller
    {
        //store the service in a private readonly field
        private readonly EducationService _educationService;

        //initialise the constructor to inject the service
        public EducationController(EducationService educationService)
        {
            _educationService = educationService;
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
        public IActionResult Create(int personId)
        {
            ViewBag.PersonId = personId;  // store it so the view can use it
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int personId, string institutionName, string degree, string fieldOfStudy, int startYear, int endYear)
        {
            if (!ModelState.IsValid) return View();
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
            return View(education);
        }

        //here form is submitted and we update the education
        //we get the education by id and then update the education
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Education/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, int personId, string institutionName, string degree, string fieldOfStudy, int startYear, int endYear)
        {
            if (!ModelState.IsValid) return View();
            var success = await _educationService.UpdateAsync(id, institutionName, degree, fieldOfStudy, startYear, endYear);
            if (!success) return NotFound();
            return RedirectToAction("Details", "Person", new { id = personId });
        }

        //delete education by
        [HttpGet]
        [Route("Education/Delete/{id:int}")]

        public async Task<IActionResult> Delete(int id)
        {
            var education = await _educationService.GetByIdAsync(id);
            if (education == null) return NotFound();
            return View(education);
        }

        [HttpPost]
        [Route("Education/Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int personId)
        {
            var success = await _educationService.DeleteAsync(id);
            if (!success) return NotFound();
            return RedirectToAction("Details", "Person", new { id = personId });
        }
     }
}
