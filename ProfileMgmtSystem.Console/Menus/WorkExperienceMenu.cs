using ProfileMgmtSystem.Console.Menus;
using ProfileMgmtSystem.Services;
using System;

namespace ProfileMgmtSystem.Console.Menus
{
    public class WorkExperienceMenu
    {
        private readonly WorkExperienceService _workExperienceService;
        private readonly PersonService _personService;

        public WorkExperienceMenu(WorkExperienceService workExperienceService, PersonService personService)
        {
            _workExperienceService = workExperienceService;
            _personService = personService;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== MANAGE WORK EXPERIENCE ===");
                System.Console.WriteLine("1. Add Work Experience");
                System.Console.WriteLine("2. Edit Work Experience");
                System.Console.WriteLine("3. Delete Work Experience");
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

            System.Console.WriteLine($"Adding work experience for {person.FullName}");
            System.Console.Write("Company Name: ");
            var company = System.Console.ReadLine() ?? "";
            System.Console.Write("Position: ");
            var position = System.Console.ReadLine() ?? "";
            System.Console.Write("Start Year: ");
            int.TryParse(System.Console.ReadLine(), out int startYear);
            System.Console.Write("End Year: ");
            int.TryParse(System.Console.ReadLine(), out int endYear);

            await _workExperienceService.CreateAsync(personId, company, position, startYear, endYear);
            System.Console.WriteLine("Work experience added successfully!");
            System.Console.ReadKey();
        }

        private async Task UpdateAsync()
        {
            System.Console.Write("Enter Work Experience ID to edit: ");
            if (!int.TryParse(System.Console.ReadLine(), out int id))
            {
                System.Console.WriteLine("Invalid ID."); System.Console.ReadKey(); return;
            }

            System.Console.Write("New Company (leave blank to skip): ");
            var company = System.Console.ReadLine();
            System.Console.Write("New Position (leave blank to skip): ");
            var position = System.Console.ReadLine();

            var success = await _workExperienceService.UpdateAsync(id,
                string.IsNullOrEmpty(company) ? null : company,
                string.IsNullOrEmpty(position) ? null : position);

            System.Console.WriteLine(success ? "Updated successfully!" : "Work experience not found.");
            System.Console.ReadKey();
        }

        private async Task DeleteAsync()
        {
            System.Console.Write("Enter Work Experience ID to delete: ");
            if (!int.TryParse(System.Console.ReadLine(), out int id))
            {
                System.Console.WriteLine("Invalid ID."); System.Console.ReadKey(); return;
            }

            System.Console.Write("Are you sure? (y/n): ");
            if (System.Console.ReadLine()?.ToLower() != "y") return;

            var success = await _workExperienceService.DeleteAsync(id);
            System.Console.WriteLine(success ? "Deleted successfully!" : "Work experience not found.");
            System.Console.ReadKey();
        }
    }
}
