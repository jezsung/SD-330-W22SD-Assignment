using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SD_330_W22SD_Assignment.Data;

namespace SD_330_W22SD_Assignment.Models
{
    public static class SeedData
    {
        public async static Task Initialize(IServiceProvider serviceProvider)
        {
            var context = new ApplicationDbContext(serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>());
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            List<string> roles = new List<string>
            {
                "Administrator", "User"
            };

            foreach (string role in roles)
            {
                var identityRole = new IdentityRole(role);

                if (!context.Roles.Contains(identityRole))
                {
                    await roleManager.CreateAsync(identityRole);
                }
            }

            await context.SaveChangesAsync();
        }
    }
}
