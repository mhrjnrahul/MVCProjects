using Microsoft.AspNetCore.Identity;
using ProfileMgmtSystem.Models;

namespace ProfileMgmtSystem.Data
{
    //static because we dont need to create an instance of this class, we just want to call its methods directly
    //this class will be used to seed the database with initial data, such as creating an admin user and some sample profiles
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            //create roles if they dont exist
            string[] roles = { "Admin", "User" };

            foreach(var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            //create admin user if it doesnt exist
            var adminEmail = "akidesuwa8@gmail.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if(adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };
                await userManager.CreateAsync(adminUser, "admin123"); //set a simple password for the admin user
                await userManager.AddToRoleAsync(adminUser, "Admin"); //assign the admin role to the admin user
            }
        }
    }
}
