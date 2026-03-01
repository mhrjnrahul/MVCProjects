using Microsoft.AspNetCore.Mvc;
using ProfileMgmtSystem.Services;

namespace ProfileMgmtSystem.Controllers
{
    public class WorkExperienceController : Controller
    {
        //store the service first
        private readonly WorkExperienceService _service;

        //initialise the constructor to inject the service
        public WorkExperienceController(WorkExperienceService service)
        {
            _service = service;
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
            return View(experience);
        }

        //form to actually edit the work experience for a person by their id
        [HttpPost]
        [Route("WorkExperience/Edit/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, int personId, string companyName, string position, int startYear, int endYear)
        {
            var success = await _service.UpdateAsync(id, companyName, position, startYear, endYear);
            if (!success)
            {
                return NotFound();
            }
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
            return View(experience);
        }

        //form to actually delete the work experience for a person by their id
        [HttpPost]
        [Route("WorkExperience/Delete/{id:int}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int personId)
        {
            var success = await _service.DeleteAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return RedirectToAction("Details", "Person", new { id = personId });
        }
    }
}
