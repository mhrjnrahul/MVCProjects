using ProfileMgmtSystem.Services;

namespace ProfileMgmtSystem.Console.Menus
{
    public class PersonMenu
    {
        private readonly PersonService _personService;

        public PersonMenu(PersonService personService)
        {
            _personService = personService;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("=== MANAGE PERSONS ===");
                System.Console.WriteLine("1. List All Persons");
                System.Console.WriteLine("2. View Person Details");
                System.Console.WriteLine("3. Create Person");
                System.Console.WriteLine("4. Update Person");
                System.Console.WriteLine("5. Delete Person");
                System.Console.WriteLine("0. Back");
                System.Console.Write("\nSelect option: ");

                var input = System.Console.ReadLine();

                switch (input)
                {
                    case "1": await ListAllAsync(); break;
                    case "2": await DetailsAsync(); break;
                    case "3": await CreateAsync(); break;
                    case "4": await UpdateAsync(); break;
                    case "5": await DeleteAsync(); break;
                    case "0": return;
                    default:
                        System.Console.WriteLine("Invalid option, press any key...");
                        System.Console.ReadKey();
                        break;
                }
            }
        }

        private async Task ListAllAsync()
        {
            System.Console.Clear();
            var persons = await _personService.ListtAllAsync();
            if (!persons.Any())
            {
                System.Console.WriteLine("No persons found.");
            }
            else
            {
                System.Console.WriteLine("=== ALL PERSONS ===");
                foreach (var p in persons)
                {
                    System.Console.WriteLine($"[{p.Id}] {p.FullName} | {p.Email} | Age: {p.Age}");
                }
            }
            System.Console.WriteLine("\nPress any key to continue...");
            System.Console.ReadKey();
        }

        private async Task DetailsAsync()
        {
            System.Console.Write("Enter Person ID: ");
            if (!int.TryParse(System.Console.ReadLine(), out int id))
            {
                System.Console.WriteLine("Invalid ID.");
                System.Console.ReadKey();
                return;
            }

            var person = await _personService.GetByIdAsync(id);
            if (person == null)
            {
                System.Console.WriteLine("Person not found.");
                System.Console.ReadKey();
                return;
            }

            System.Console.Clear();
            System.Console.WriteLine($"=== {person.FullName} ===");
            System.Console.WriteLine($"Email: {person.Email}");
            System.Console.WriteLine($"Age: {person.Age}");
            System.Console.WriteLine($"Phone: {person.PhoneNumber}");

            System.Console.WriteLine("\n-- Education --");
            foreach (var e in person.Educations)
                System.Console.WriteLine($"  {e.Degree} in {e.FieldOfStudy} at {e.InstitutionName} ({e.StartYear}-{e.EndYear})");

            System.Console.WriteLine("\n-- Skills --");
            foreach (var s in person.Skills)
                System.Console.WriteLine($"  {s.Name} ({s.Category}) - {s.Proficiency}");

            System.Console.WriteLine("\n-- Work Experience --");
            foreach (var w in person.Experiences)
                System.Console.WriteLine($"  {w.Position} at {w.CompanyName} ({w.StartYear}-{w.EndYear})");

            System.Console.WriteLine("\nPress any key to continue...");
            System.Console.ReadKey();
        }

        private async Task CreateAsync()
        {
            System.Console.Clear();
            System.Console.WriteLine("=== CREATE PERSON ===");

            System.Console.Write("First Name: ");
            var firstName = System.Console.ReadLine() ?? "";

            System.Console.Write("Last Name: ");
            var lastName = System.Console.ReadLine() ?? "";

            System.Console.Write("Email: ");
            var email = System.Console.ReadLine() ?? "";

            System.Console.Write("Date of Birth (yyyy-MM-dd): ");
            if (!DateTime.TryParse(System.Console.ReadLine(), out DateTime dob))
            {
                System.Console.WriteLine("Invalid date.");
                System.Console.ReadKey();
                return;
            }

            var person = await _personService.CreateAsync(firstName, lastName, email, dob);
            System.Console.WriteLine($"\nCreated: {person.FullName} with ID {person.Id}");
            System.Console.ReadKey();
        }

        private async Task UpdateAsync()
        {
            System.Console.Write("Enter Person ID to update: ");
            if (!int.TryParse(System.Console.ReadLine(), out int id))
            {
                System.Console.WriteLine("Invalid ID.");
                System.Console.ReadKey();
                return;
            }

            System.Console.Write("New Email (leave blank to skip): ");
            var email = System.Console.ReadLine();

            System.Console.Write("New First Name (leave blank to skip): ");
            var firstName = System.Console.ReadLine();

            var success = await _personService.UpdateAsync(id,
                string.IsNullOrEmpty(email) ? null : email,
                string.IsNullOrEmpty(firstName) ? null : firstName);

            System.Console.WriteLine(success ? "Updated successfully!" : "Person not found.");
            System.Console.ReadKey();
        }

        private async Task DeleteAsync()
        {
            System.Console.Write("Enter Person ID to delete: ");
            if (!int.TryParse(System.Console.ReadLine(), out int id))
            {
                System.Console.WriteLine("Invalid ID.");
                System.Console.ReadKey();
                return;
            }

            System.Console.Write("Are you sure? (y/n): ");
            if (System.Console.ReadLine()?.ToLower() != "y") return;

            var success = await _personService.DeleteAsync(id);
            System.Console.WriteLine(success ? "Deleted successfully!" : "Person not found.");
            System.Console.ReadKey();
        }
    }
}