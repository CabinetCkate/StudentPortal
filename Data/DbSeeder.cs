using Microsoft.AspNetCore.Identity;
using StudentPortal.Models;

namespace StudentPortal.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(IServiceProvider services, IConfiguration configuration)
    {
        // RoleManager жүйедегі рөлдерді қайталамай құрып, тексеру үшін қолданылады.
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();

        foreach (var roleName in new[] { "Admin", "Student" })
        {
            if (await roleManager.RoleExistsAsync(roleName))
            {
                continue;
            }

            var roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"'{roleName}' рөлін құру мүмкін болмады: {string.Join("; ", roleResult.Errors.Select(error => error.Description))}");
            }
        }

        var adminEmail = configuration["SeedAdmin:Email"];
        var adminPassword = configuration["SeedAdmin:Password"];
        if (string.IsNullOrWhiteSpace(adminEmail) || string.IsNullOrWhiteSpace(adminPassword))
        {
            // Admin credentials are optional for local development. The application
            // remains usable for student registration and the Admin role is created above.
            return;
        }

        var admin = await userManager.FindByEmailAsync(adminEmail);
        if (admin is null)
        {
            admin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Жүйе",
                LastName = "Әкімшісі",
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(admin, adminPassword);
            if (!createResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Бастапқы әкімшіні құру мүмкін болмады: {string.Join("; ", createResult.Errors.Select(error => error.Description))}");
            }
        }

        if (!await userManager.IsInRoleAsync(admin, "Admin"))
        {
            var addRoleResult = await userManager.AddToRoleAsync(admin, "Admin");
            if (!addRoleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    $"Бастапқы әкімшіге Admin рөлін беру мүмкін болмады: {string.Join("; ", addRoleResult.Errors.Select(error => error.Description))}");
            }
        }
    }
}
