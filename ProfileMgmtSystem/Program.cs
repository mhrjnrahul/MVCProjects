using Microsoft.EntityFrameworkCore;
using ProfileMgmtSystem.Data;
using ProfileMgmtSystem.Services;
using ProfileMgmtSystem.Repositories;

var connectionString =

    "Server=FOREVERWINTER;Database=ProfileMIS(semis);Trusted_Connection=True;TrustServerCertificate=True;";

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>(); //we register the unit of work as a scoped service,
                                                       //which means that a new instance of the unit of work will be created for each HTTP request and shared across all services that require it during that request
builder.Services.AddScoped<PersonService>();
builder.Services.AddScoped<EducationService>();
builder.Services.AddScoped<SkillService>();
builder.Services.AddScoped<WorkExperienceService>();

//register dbcontext
builder.Services.AddDbContext<ProfileDbContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Person}/{action=Index}/{id?}");

app.Run();
