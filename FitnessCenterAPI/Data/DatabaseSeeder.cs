using Microsoft.AspNetCore.Identity;

namespace FitnessCenterApi.Data;

public static class DatabaseSeeder
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles = { "Client" }; // <- Your role(s) you want to make sure exist

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new IdentityRole(role));
                Console.WriteLine($"Role '{role}' created.");
            }
            else
            {
                Console.WriteLine($"Role '{role}' already exists.");
            }
        }
    }
}
