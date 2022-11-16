using AppLevelAuthorization.Application.Identity;
using MediatR;

namespace AppLevelAuthorization.Application.Queries.RoleQueries;

[Authorize(Role = EnRole.Manager)]
public record ManagerQuery() : IRequest<string>;

public class ManagerQueryHandler : IRequestHandler<ManagerQuery, string>
{
    public Task<string> Handle(ManagerQuery request, CancellationToken cancellationToken) 
        => Task.FromResult("Hello Manager");
}