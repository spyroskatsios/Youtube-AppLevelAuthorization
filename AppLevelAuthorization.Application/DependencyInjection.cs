using System.Reflection;
using AppLevelAuthorization.Application.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace AppLevelAuthorization.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(Assembly.GetExecutingAssembly());
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
        
        return services;
    }
}