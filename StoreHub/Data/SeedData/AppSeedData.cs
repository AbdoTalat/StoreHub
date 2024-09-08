using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using StoreHub.Data.DbContext;
using StoreHub.Models.Entities;

namespace StoreHub.Data.SeedData
{
    public static class AppSeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<StoreHubContext>();
            using var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            using var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            context.Database.EnsureCreated();

            // Seed Categories
            if (!context.Categories.Any())
            {
                var categories = await ReadFromFile<List<Category>>("Categories.json");
                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // Seed Products
            if (!context.Products.Any())
            {
                var products = await ReadFromFile<List<Product>>("Products.json");
                context.Products.AddRange(products);
                await context.SaveChangesAsync();
            }

            // Seed Roles
            var roles = await ReadFromFile<List<string>>("Roles.json");
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Seed Admin User
            var users = await ReadFromFile<List<UserDTO>>("AppUser.json");

            foreach (var user in users)
            {
                if (!await userManager.Users.AnyAsync(u => u.UserName == user.UserName))
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = user.UserName,
                        Email = user.Email,
                    };
                    var result = await userManager.CreateAsync(adminUser, user.Password);
                    if (result.Succeeded)
                    {
                        var rolesToAsign = await roleManager.Roles.ToListAsync();
                        if (rolesToAsign.Any(r => r.Name == "Admin"))
                            await userManager.AddToRoleAsync(adminUser, "Admin");
                    }

                }
            }
        }
        private static async Task<T> ReadFromFile<T>(string path)
        {
            var jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "SeedData", path);
            var jsonData = await File.ReadAllTextAsync(jsonFilePath);
            return JsonConvert.DeserializeObject<T>(jsonData);
        }

    }
}
public class UserDTO
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}