using AppLevelAuthorization.Application.ServiceResults;

namespace AppLevelAuthorization.Application.Interfaces;

public interface IIdentityService
{
    Task<AuthResult> SignInUserAsync(string email, string password);
    Task<AuthResult> SignUpUserAsync(string email, string userName, string password);
    Task<AuthResult> AddUserToRoleAsync(string userId, string roleName);
    Task CreateRolesAsync(IEnumerable<string> roles);
}
