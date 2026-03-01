using ProfileMgmtSystem.Models;
using ProfileMgmtSystem.Services;

namespace ProfileMgmtSystem.Console.Menus
{
    public class SkillMenu
    {
        private readonly SkillService _skillService;
        private readonly PersonService _personService;

        public SkillMenu(SkillService skillService, PersonService personService)
        {
            _skillService = skillService;
            _personService = personService;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== MANAGE SKILLS ===");
                System.Console.WriteLine("1. Add Skill");
                System.Console.WriteLine("2. Edit Skill");
                System.Console.WriteLine("3. Delete Skill");
                System.Console.WriteLine("0. Back");
                System.Console.Write("\nSelect option: ");

                switch (System.Console.ReadLine())
                {
                    case "1": await CreateAsync(); break;
                    case "2": await UpdateAsync(); break;
                    case "3": await DeleteAsync(); break;
                    case "0": return;
                    default:
                        System.Console.WriteLine("Invalid option, press any key...");
                        System.Console.ReadKey();
                        break;
                }
            }
        }

        private async Task CreateAsync()
        {
            System.Console.Write("Enter Person ID: ");
            if (!int.TryParse(System.Console.ReadLine(), out int personId))
            {
                System.Console.WriteLine("Invalid ID."); System.Console.ReadKey(); return;
            }

            var person = await _personService.GetByIdAsync(personId);
            if (person == null)
            {
                System.Console.WriteLine("Person not found."); System.Console.ReadKey(); return;
            }

            System.Console.WriteLine($"Adding skill for {person.FullName}");
            System.Console.Write("Skill Name: ");
            var name = System.Console.ReadLine() ?? "";
            System.Console.Write("Category: ");
            var category = System.Console.ReadLine() ?? "";

            System.Console.WriteLine("Proficiency (0=Beginner, 1=Intermediate, 2=Advanced, 3=Expert): ");
            Enum.TryParse<ProficiencyLevel>(System.Console.ReadLine(), out var proficiency);

            await _skillService.CreateAsync(personId, name, category, proficiency);
            System.Console.WriteLine("Skill added successfully!");
            System.Console.ReadKey();
        }

        private async Task UpdateAsync()
        {
            System.Console.Write("Enter Skill ID to edit: ");
            if (!int.TryParse(System.Console.ReadLine(), out int id))
            {
                System.Console.WriteLine("Invalid ID."); System.Console.ReadKey(); return;
            }

            System.Console.Write("New Name (leave blank to skip): ");
            var name = System.Console.ReadLine();
            System.Console.Write("New Category (leave blank to skip): ");
            var category = System.Console.ReadLine();

            var success = await _skillService.UpdateAsync(id,
                string.IsNullOrEmpty(name) ? null : name,
                string.IsNullOrEmpty(category) ? null : category);

            System.Console.WriteLine(success ? "Updated successfully!" : "Skill not found.");
            System.Console.ReadKey();
        }

        private async Task DeleteAsync()
        {
            System.Console.Write("Enter Skill ID to delete: ");
            if (!int.TryParse(System.Console.ReadLine(), out int id))
            {
                System.Console.WriteLine("Invalid ID."); System.Console.ReadKey(); return;
            }

            System.Console.Write("Are you sure? (y/n): ");
            if (System.Console.ReadLine()?.ToLower() != "y") return;

            var success = await _skillService.DeleteAsync(id);
            System.Console.WriteLine(success ? "Deleted successfully!" : "Skill not found.");
            System.Console.ReadKey();
        }
    }
}