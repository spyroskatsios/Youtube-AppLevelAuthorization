using System.Security.Claims;
using AppLevelAuthorization.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AppLevelAuthorization.Infrastructure.Services;


public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }
    
    public string? UserId => _httpContextAccessor?.HttpContext?.User?.Claims?.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;

    public IEnumerable<string> Roles => _httpContextAccessor?.HttpContext?.User?.Claims?
        .Where(x => x.Type == ClaimTypes.Role)
        .Select(x => x.Value) ?? Enumerable.Empty<string>();
}