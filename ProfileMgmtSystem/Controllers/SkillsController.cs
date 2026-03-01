using Microsoft.AspNetCore.Mvc;
using ProfileMgmtSystem.Services;
using ProfileMgmtSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace ProfileMgmtSystem.Controllers
{
    [Authorize]
    public class SkillsController : Controller
    {
        //store service
        private readonly SkillService _skillService;
        private readonly PersonService _personService; 
        private readonly UserManager<ApplicationUser> _userManager;

        //intialise constructor to inject the service
        public SkillsController(SkillService skillService, PersonService personService, UserManager<ApplicationUser> userManager)
        {
            _skillService = skillService;
            _personService = personService;
            _userManager = userManager;
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
            if(!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                var person = await _personService.GetByUserIdAsync(userId!);
                if(person == null || person.Id != personId)
                {
                    return Forbid(); //403 forbidden if they try to add skill to someone else's profile
                }
            }
            await _skillService.CreateAsync(personId, skillName, category, proficiency);
            return RedirectToAction("Details", "Person", new { id = personId });
        }

        //edit the skill for a person by their id
        [HttpGet]
        [Route("Skill/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id)
        {
            var skill = await _skillService.GetByIdAsync(id);
            if (skill == null)
            {
                return NotFound();
            }

            if(!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                var person = await _personService.GetByUserIdAsync(userId!);
                if (person == null || person.Id != skill.PersonId)
                {
                    return Forbid(); //403 forbidden if they try to edit skill of someone else's profile
                }
            }

            ViewBag.ProficiencyLevels = Enum.GetValues(typeof(ProficiencyLevel));
            return View(skill);
        }

        //form to actually edit the skill for a person by their id
        [HttpPost]
        [Route("Skill/Edit/{id:int}")]
        public async Task<IActionResult> Edit(int id, int personId, string skillName, string category, ProficiencyLevel proficiency)
        {
            var skill = await _skillService.GetByIdAsync(id);
            if(skill == null)
            {
                return NotFound();
            }

            if(!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                var person = await _personService.GetByUserIdAsync(userId!);
                if (person == null || person.Id != skill.PersonId)
                {
                    return Forbid(); //403 forbidden if they try to edit skill of someone else's profile
                }
            }
            await _skillService.UpdateAsync(id, skillName, category, proficiency);
            return RedirectToAction("Details", "Person", new { id = personId });
        }

        //delete the skill for a person by their id
        [HttpGet]
        [Route("Skill/Delete/{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var skill = await _skillService.GetByIdAsync(id);
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
            var skill = await _skillService.GetByIdAsync(id);
            if (skill == null)
            {
                return NotFound();
            }

            if(!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                var person = await _personService.GetByUserIdAsync(userId!);
                if (person == null || person.Id != skill.PersonId)
                {
                    return Forbid(); //403 forbidden if they try to delete skill of someone else's profile
                }
            }
            await _skillService.DeleteAsync(id);
            return RedirectToAction("Details", "Person", new { id = personId });
        }
    }   
}
