using Microsoft.EntityFrameworkCore;
using ProfileMgmtSystem.Data;
using ProfileMgmtSystem.Services;
using ProfileMgmtSystem.Repositories;
using Microsoft.AspNetCore.Identity;
using ProfileMgmtSystem.Models;

var connectionString =

    "Server=FOREVERWINTER;Database=ProfileMIS(semis);Trusted_Connection=True;TrustServerCertificate=True;";

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();

//add identity services for user authentication and authorization
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    //simple password rules for now
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
    .AddEntityFrameworkStores<ProfileDbContext>()
    .AddDefaultTokenProviders();

//configure login logout paths
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});


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

//add authentication and authorization middleware to the pipeline
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Person}/{action=Index}/{id?}");

// seed admin user on startup
using (var scope = app.Services.CreateScope())
{
    await SeedData.InitializeAsync(scope.ServiceProvider);
}

app.Run();
