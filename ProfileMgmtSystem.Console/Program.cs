using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProfileMgmtSystem.Data;
using ProfileMgmtSystem.Repositories;
using ProfileMgmtSystem.Services;
using ProfileMgmtSystem.Console.Menus;

// setup dependency injection - same as in MVC project
var services = new ServiceCollection();

var connectionString = "Server=FOREVERWINTER;Database=ProfileMIS(semis);Trusted_Connection=True;TrustServerCertificate=True;";

services.AddDbContext<ProfileDbContext>(options =>
    options.UseSqlServer(connectionString));

services.AddScoped<IUnitOfWork, UnitOfWork>();
services.AddScoped<PersonService>();
services.AddScoped<EducationService>();
services.AddScoped<SkillService>();
services.AddScoped<WorkExperienceService>();

var serviceProvider = services.BuildServiceProvider();

// start the app
using var scope = serviceProvider.CreateScope();
var app = new MainMenu(
    scope.ServiceProvider.GetRequiredService<PersonService>(),
    scope.ServiceProvider.GetRequiredService<EducationService>(),
    scope.ServiceProvider.GetRequiredService<SkillService>(),
    scope.ServiceProvider.GetRequiredService<WorkExperienceService>()
);

await app.RunAsync();