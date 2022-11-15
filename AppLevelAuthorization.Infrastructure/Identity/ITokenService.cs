namespace AppLevelAuthorization.Infrastructure.Identity;

public interface ITokenService
{
    Task<string> GenerateTokenAsync(AppUser user);
}