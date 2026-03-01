using ProfileMgmtSystem.Services;

namespace ProfileMgmtSystem.Console.Menus
{
    public class MainMenu
    {
        private readonly PersonService _personService;
        private readonly EducationService _educationService;
        private readonly SkillService _skillService;
        private readonly WorkExperienceService _workExperienceService;

        public MainMenu(PersonService personService, EducationService educationService, SkillService skillService, WorkExperienceService workExperienceService)
        {
            _personService = personService;
            _educationService = educationService;
            _skillService = skillService;
            _workExperienceService = workExperienceService;
        }

        public async Task RunAsync()
        {
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine("================================");
                System.Console.WriteLine("   PROFILE MANAGEMENT SYSTEM   ");
                System.Console.WriteLine("================================");
                System.Console.WriteLine("1. Manage Persons");
                System.Console.WriteLine("2. Manage Education");
                System.Console.WriteLine("3. Manage Skills");
                System.Console.WriteLine("4. Manage Work Experience");
                System.Console.WriteLine("0. Exit");
                System.Console.Write("\nSelect option: ");

                var input = System.Console.ReadLine();

                switch (input)
                {
                    case "1":
                        var personMenu = new PersonMenu(_personService);
                        await personMenu.RunAsync();
                        break;
                    case "2":
                        var educationMenu = new EducationMenu(_educationService, _personService);
                        await educationMenu.RunAsync();
                        break;
                    case "3":
                        var skillMenu = new SkillMenu(_skillService, _personService);
                        await skillMenu.RunAsync();
                        break;
                    case "4":
                        var workMenu = new WorkExperienceMenu(_workExperienceService, _personService);
                        await workMenu.RunAsync();
                        break;
                    case "0":
                        System.Console.WriteLine("Goodbye!");
                        return;
                    default:
                        System.Console.WriteLine("Invalid option, press any key to try again...");
                        System.Console.ReadKey();
                        break;
                }
            }
        }
    }
}