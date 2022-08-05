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

            if (context.Users.FirstOrDefault(u => u.Email == "admin@test.com") == null)
            {
                var admin = new ApplicationUser
                {
                    Email = "admin@test.com",
                    NormalizedEmail = "ADMIN@TEST.COM",
                    UserName = "admin@test.com",
                    NormalizedUserName = "ADMIN@TEST.COM",
                    EmailConfirmed = true,
                };
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(admin, "Password1@");
                admin.PasswordHash = hashed;


                await userManager.CreateAsync(admin);
                await userManager.AddToRoleAsync(admin, "Administrator");
            }

            if (context.Users.FirstOrDefault(u => u.Email == "user1@test.com") == null)
            {
                var user1 = new ApplicationUser
                {
                    Email = "user1@test.com",
                    NormalizedEmail = "USER1@TEST.COM",
                    UserName = "user1@test.com",
                    NormalizedUserName = "USER1@TEST.COM",
                    EmailConfirmed = true,
                };
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user1, "Password1@");
                user1.PasswordHash = hashed;


                await userManager.CreateAsync(user1);
                await userManager.AddToRoleAsync(user1, "User");
            }

            if (context.Users.FirstOrDefault(u => u.Email == "user2@test.com") == null)
            {
                var user1 = new ApplicationUser
                {
                    Email = "user2@test.com",
                    NormalizedEmail = "USER2@TEST.COM",
                    UserName = "user2@test.com",
                    NormalizedUserName = "USER2@TEST.COM",
                    EmailConfirmed = true,
                };
                var password = new PasswordHasher<ApplicationUser>();
                var hashed = password.HashPassword(user1, "Password1@");
                user1.PasswordHash = hashed;


                await userManager.CreateAsync(user1);
                await userManager.AddToRoleAsync(user1, "User");
            }

            await context.SaveChangesAsync();
        }
    }
}
