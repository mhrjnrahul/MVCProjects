using ProfileMgmtSystem.Services;

namespace ProfileMgmtSystem.Console.Menus
{
    public class EducationMenu
    {
        private readonly EducationService _educationService;
        private readonly PersonService _personService;

        public EducationMenu(EducationService educationService, PersonService personService)
        {
            _educationService = educationService;
            _personService = personService;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== MANAGE EDUCATION ===");
                System.Console.WriteLine("1. Add Education");
                System.Console.WriteLine("2. Edit Education");
                System.Console.WriteLine("3. Delete Education");
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

            System.Console.WriteLine($"Adding education for {person.FullName}");
            System.Console.Write("Institution Name: ");
            var institution = System.Console.ReadLine() ?? "";
            System.Console.Write("Degree: ");
            var degree = System.Console.ReadLine() ?? "";
            System.Console.Write("Field of Study: ");
            var field = System.Console.ReadLine() ?? "";
            System.Console.Write("Start Year: ");
            int.TryParse(System.Console.ReadLine(), out int startYear);
            System.Console.Write("End Year: ");
            int.TryParse(System.Console.ReadLine(), out int endYear);

            await _educationService.CreateAsync(personId, institution, degree, field, startYear, endYear);
            System.Console.WriteLine("Education added successfully!");
            System.Console.ReadKey();
        }

        private async Task UpdateAsync()
        {
            System.Console.Write("Enter Education ID to edit: ");
            if (!int.TryParse(System.Console.ReadLine(), out int id))
            {
                System.Console.WriteLine("Invalid ID."); System.Console.ReadKey(); return;
            }

            System.Console.Write("New Institution (leave blank to skip): ");
            var institution = System.Console.ReadLine();
            System.Console.Write("New Degree (leave blank to skip): ");
            var degree = System.Console.ReadLine();
            System.Console.Write("New Field of Study (leave blank to skip): ");
            var field = System.Console.ReadLine();

            var success = await _educationService.UpdateAsync(id,
                string.IsNullOrEmpty(institution) ? null : institution,
                string.IsNullOrEmpty(degree) ? null : degree,
                string.IsNullOrEmpty(field) ? null : field);

            System.Console.WriteLine(success ? "Updated successfully!" : "Education not found.");
            System.Console.ReadKey();
        }

        private async Task DeleteAsync()
        {
            System.Console.Write("Enter Education ID to delete: ");
            if (!int.TryParse(System.Console.ReadLine(), out int id))
            {
                System.Console.WriteLine("Invalid ID."); System.Console.ReadKey(); return;
            }

            System.Console.Write("Are you sure? (y/n): ");
            if (System.Console.ReadLine()?.ToLower() != "y") return;

            var success = await _educationService.DeleteAsync(id);
            System.Console.WriteLine(success ? "Deleted successfully!" : "Education not found.");
            System.Console.ReadKey();
        }
    }
}