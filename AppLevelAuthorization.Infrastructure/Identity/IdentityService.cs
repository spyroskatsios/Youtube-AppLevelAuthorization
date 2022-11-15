using AppLevelAuthorization.Application.Interfaces;
using AppLevelAuthorization.Application.ServiceResults;
using Microsoft.AspNetCore.Identity;

namespace AppLevelAuthorization.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly RoleManager<IdentityRole> _roleManager;

    public IdentityService(UserManager<AppUser> userManager, ITokenService tokenService, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _roleManager = roleManager;
    }

    public async Task<AuthResult> SignInUserAsync(string email, string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return new AuthResult { Success = false, Errors = new[] { "Wrong email or password." } };
        }

        if (!(await _userManager.CheckPasswordAsync(user, password)))
        {
            return new AuthResult { Success = false, Errors = new[] { "Wrong email or password." } };
        }


        var token = await _tokenService.GenerateTokenAsync(user);

        return new AuthResult { Success = true, Token = token };
    }

    public async Task<AuthResult> SignUpUserAsync(string email, string userName, string password)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser is not null)
        {
            return new AuthResult { Success = false, Errors = new string[] { "User already exists." } };
        }

        var user = new AppUser
        {
            Email = email,
            UserName = userName
        };

        var result = await _userManager.CreateAsync(user, password);
        
        return !result.Succeeded 
            ? new AuthResult { Success = false, Errors = result.Errors.Select(e => e.Description) } 
            : new AuthResult { Success = true };
    }
    
    public async Task<AuthResult> AddUserToRoleAsync(string userId, string roleName)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null)
            return new AuthResult { Success = false, Errors = new[] { "User not found" } };
        

        var role = await _roleManager.FindByNameAsync(roleName);

        if (role is null)
            return new AuthResult { Success = false, Errors = new[] { "Role not found" } };

        var result = await _userManager.AddToRoleAsync(user, roleName);

        return new AuthResult { Success = result.Succeeded, Errors = result.Errors.Select(x => x.Description) };
    }
    
    public async Task CreateRolesAsync(IEnumerable<string> roles)
    {
        foreach (var role in roles)
        {
            var roleExists = await _roleManager.RoleExistsAsync(role);

            if (!roleExists)
                await _roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}