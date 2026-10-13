using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProductCatalogApp.Models;

namespace ProductCatalogApp.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(
            IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            var passwordHasher = scope.ServiceProvider
                .GetRequiredService<IPasswordHasher<AppUser>>();

            await context.Database.MigrateAsync();

            var adminRole = await context.Roles
                .FirstOrDefaultAsync(role => role.Name == "Admin");

            if (adminRole == null)
            {
                adminRole = new Role
                {
                    Name = "Admin"
                };

                context.Roles.Add(adminRole);
                await context.SaveChangesAsync();
            }

            var admin = await context.Users
                .FirstOrDefaultAsync(
                    user => user.Username == "admin");

            if (admin == null)
            {
                admin = new AppUser
                {
                    Username = "admin",
                    RoleId = adminRole.Id
                };

                admin.PasswordHash =
                    passwordHasher.HashPassword(
                        admin,
                        "Admin123!");

                context.Users.Add(admin);
                await context.SaveChangesAsync();
            }
            else if (admin.RoleId != adminRole.Id)
            {
                admin.RoleId = adminRole.Id;
                await context.SaveChangesAsync();
            }
        }
    }
}