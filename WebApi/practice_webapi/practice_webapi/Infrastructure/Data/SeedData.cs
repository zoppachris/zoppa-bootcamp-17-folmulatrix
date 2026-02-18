using Microsoft.AspNetCore.Identity;
using practice_webapi.Domain.Entities;

namespace practice_webapi.Infrastructure.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

            // Create roles
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }
            }

            // Create admin user
            var adminEmail = "admin@todoapi.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new AppUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "API",
                    LastName = "Administrator",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(adminUser, "Admin123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                    Console.WriteLine($"Admin user created successfully with email: {adminEmail}");
                    Console.WriteLine("Default password: Admin123!");
                }
            }

            // Create sample user
            var userEmail = "user@todoapi.com";
            var sampleUser = await userManager.FindByEmailAsync(userEmail);

            if (sampleUser == null)
            {
                sampleUser = new AppUser
                {
                    UserName = userEmail,
                    Email = userEmail,
                    FirstName = "API",
                    LastName = "User",
                    EmailConfirmed = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(sampleUser, "User123!");

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(sampleUser, "User");
                    Console.WriteLine($"Sample user created successfully with email: {userEmail}");
                    Console.WriteLine("Default password: User123!");
                }
            }
        }
    }
}
