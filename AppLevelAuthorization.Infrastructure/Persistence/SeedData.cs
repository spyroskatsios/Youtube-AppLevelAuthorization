using AppLevelAuthorization.Application.Interfaces;
using AppLevelAuthorization.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace AppLevelAuthorization.Infrastructure.Persistence;

public class SeedData
{
    private readonly IConfiguration _config;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public SeedData(IConfiguration config, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _config = config;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    
    public async Task SetUpRoles()
    {
        var roles = _config.GetSection("Roles").Get<IEnumerable<string>>();

        foreach (var role in roles)
        {
            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new IdentityRole(role));
            }
        }
    }

    public async Task CreateAdminsAsync()
    {
        var admins = _config.GetSection("Admins").Get<IEnumerable<AppUser>>();

        foreach (var user in admins)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser is not null) continue;

            await _userManager.CreateAsync(user, "Pass123!");

            await _userManager.AddToRoleAsync(user, "Admin");
        }
    }
    
    public async Task CreateManagersAsync()
    {
        var admins = _config.GetSection("Managers").Get<IEnumerable<AppUser>>();

        foreach (var user in admins)
        {
            var existingUser = await _userManager.FindByEmailAsync(user.Email);

            if (existingUser is not null) continue;

            await _userManager.CreateAsync(user, "Pass123!");

            await _userManager.AddToRoleAsync(user, "Manager");
        }
    }

}