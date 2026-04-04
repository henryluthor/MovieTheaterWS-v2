using Microsoft.AspNetCore.Identity;
using MovieTheaterWS_v2.Models;
using Microsoft.Extensions.DependencyInjection;

namespace MovieTheaterWS_v2.Data
{
    public class DbSeeder
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider service)
        {
            var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = service.GetRequiredService<UserManager<User>>();
            var logger = service.GetRequiredService<ILogger<DbSeeder>>();

            // Create roles only if they do not exist
            string[] roles = { "Admin", "Customer" };
            foreach (var rol in roles)
            {
                if (!await roleManager.RoleExistsAsync(rol))
                {
                    await roleManager.CreateAsync(new IdentityRole(rol));
                }
            }

            // Create initial Admin only if it does not exist
            var adminEmail = "brunokane7@yahoo.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var admin = new User
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(admin, "Admin123-");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                    logger.LogInformation("Administrator user created successfully.");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        //Console.WriteLine($"Error creating admin: {error.Description}");
                        logger.LogError($"Error creating admin: {error.Description}");
                    }
                }
            }
        }
    }
}
