using Internal.Domain.Entities;
using Internal.Infrastructure.Persistance;
using Microsoft.AspNetCore.Identity;

namespace Internal.API.Seeders;

public class DbInitializer
{
    private readonly MovieDbContext _context;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly UserManager<User> _userManager;

    public DbInitializer(UserManager<User> userManager, 
        MovieDbContext context,
        RoleManager<IdentityRole<int>> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitializeAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        if (!_context.Users.Any())
        {
            await _roleManager.CreateAsync(new IdentityRole<int> { Name = "Admin" });
            await _roleManager.CreateAsync(new IdentityRole<int> { Name = "User" });

            await _context.SaveChangesAsync();

            var user = new User
            {
                UserName = "admin",
                Email = "admin@gmail.com",
            };

            var result = await _userManager.CreateAsync(user, "User!123");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }

            await _context.SaveChangesAsync();
        }
    }
}