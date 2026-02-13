using Microsoft.AspNetCore.Identity;

namespace TaskManagement.Infrastructure.Persistence
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(AppDbContext context, RoleManager<IdentityRole<Guid>> roleManager)
        {
            await context.Database.EnsureCreatedAsync();

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
            }

            if (!await roleManager.RoleExistsAsync("User"))
            {
                await roleManager.CreateAsync(new IdentityRole<Guid>("User"));
            }
        }
    }
}