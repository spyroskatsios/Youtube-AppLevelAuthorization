using System.Reflection;
using AppLevelAuthorization.Application.Exceptions;
using AppLevelAuthorization.Application.Identity;
using AppLevelAuthorization.Application.Interfaces;
using MediatR;

namespace AppLevelAuthorization.Application.Behaviors;

public class AuthorizationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private ICurrentUserService _currentUserService;

    public AuthorizationBehavior(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var authorizedAttributes = request.GetType().GetCustomAttributes<AuthorizeAttribute>()
            .Where(x => x.Role > 0).ToList();

        if (!authorizedAttributes.Any())
            return await next();

        var userRoles = _currentUserService.Roles.ToList();

        foreach (var role in authorizedAttributes.Select(x=>x.Role))
        {
            if (IsAuthorized(role, userRoles))
                return await next();
        }

        throw new ForbiddenException();
    }

    private static bool IsAuthorized(EnRole requiredRole, List<string> userRoles)
        => userRoles.Contains(requiredRole.ToString()) || userRoles.Any(x => Enum.Parse<EnRole>(x) < requiredRole); // In our scenario, we have a hierarchy of roles, so if the user has a role with a lower value, he is also authorized

}