using Microsoft.AspNetCore.Mvc;
using ProfileMgmtSystem.Services;
using ProfileMgmtSystem.Models;
using System.Threading.Tasks;

namespace ProfileMgmtSystem.Controllers
{
    public class PersonController : Controller
    {
        
        private readonly PersonService _personService;

        public PersonController(PersonService personService)
        {
            _personService = personService;
        }

        //get all person
        //get the persons and then return them in the view
        public async Task<IActionResult> Index()
        {
            var persons = await _personService.ListtAllAsync();
            return View(persons);
        }

        //get a person detail by id
        //details.cshtml
        public async Task<IActionResult> Details(int id)
        {
            var person = await _personService.GetByIdAsync(id);
            if (person == null) return NotFound();
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string firstName, string lastName, string email, DateTime dateOfBirth)
        {
            if (!ModelState.IsValid) return View();

            await _personService.CreateAsync(firstName, lastName, email, dateOfBirth);
            return RedirectToAction(nameof(Index));
        }

        //edit method
        //edit by id of person
        //u get the person by id and then return the person in the view to edit
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var person = await _personService.GetByIdAsync(id);
            if (person == null) return NotFound();
            return View(person);
        }

        //post method for edit
        //here u get the form data and then update the person in the database
        [HttpPost]
        public async Task<IActionResult> Edit(int id, string? email = null, string? firstName = null)
        {
            if (!ModelState.IsValid) return View();
            var success = await _personService.UpdateAsync(id, email, firstName);
            if (!success) return NotFound();
            //nameof is a way to get the name of a method as a string, which is useful for refactoring and avoiding hard-coded strings
            //if we change the name of the Index method, this will automatically update the string in RedirectToAction, preventing errors and improving maintainability
            return RedirectToAction(nameof(Index));
        }

        //delete method
        //get the person by id and then return the person in the view to confirm deletion
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var person = await _personService.GetByIdAsync(id);
            if (person == null) return NotFound();

            return View(person);
        }

        //post method for delete
        [HttpPost, ActionName("Delete")]
        //action name is used to specify the name of the action method that will handle the form submission, which is useful when the method name does not match the action name in the view
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var success = await _personService.DeleteAsync(id);
            if (!success) return NotFound();
            return RedirectToAction(nameof(Index));

        }
    }
}