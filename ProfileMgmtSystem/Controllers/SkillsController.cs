using Microsoft.AspNetCore.Mvc;
using ProfileMgmtSystem.Services;
using ProfileMgmtSystem.Models;


namespace ProfileMgmtSystem.Controllers
{
    public class SkillsController : Controller
    {
        //store service
        private readonly SkillService _service;

        //intialise constructor to inject the service
        public SkillsController(SkillService service)
        {
            _service = service;
        }

        //create skill for a person
        [HttpGet]
        public IActionResult Create(int personId)
        {
            //you need to viewbag person id
            ViewBag.PersonId = personId;
            ViewBag.ProficiencyLevels = Enum.GetValues(typeof(ProficiencyLevel));
            return View();
        }

        //show the create form for skill
        //you create a skill for a person with their id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int personId, string skillName, string category, ProficiencyLevel proficiency)
        {
            await _service.CreateAsync(personId, skillName, category, proficiency);
            return RedirectToAction("Details", "Person", new { id = personId });
        }

        //edit the skill for a person by their id
        [HttpGet]
        [Route("Skill/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var skill = await _service.GetByIdAsync(id);
            if (skill == null)
            {
                return NotFound();
            }
            ViewBag.ProficiencyLevels = Enum.GetValues(typeof(ProficiencyLevel));
            return View(skill);
        }

        //form to actually edit the skill for a person by their id
        [HttpPost]
        [Route("Skill/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, int personId, string skillName, string category, ProficiencyLevel proficiency)
        {
            var success = await _service.UpdateAsync(id, skillName, category, proficiency);
            if (!success)
            {
                return NotFound();
            }
            return RedirectToAction("Details", "Person", new { id = personId });
        }

        //delete the skill for a person by their id
        [HttpGet]
        [Route("Skill/Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var skill = await _service.GetByIdAsync(id);
            if (skill == null)
            {
                return NotFound();
            }
            return View(skill);
        }

        //form to actually delete the skill for a person by their id
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Skill/Delete/{id:int}")]
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
