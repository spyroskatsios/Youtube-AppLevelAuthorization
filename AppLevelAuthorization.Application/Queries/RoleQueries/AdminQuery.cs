using AppLevelAuthorization.Application.Identity;
using MediatR;

namespace AppLevelAuthorization.Application.Queries.RoleQueries;

[Authorize(Role = EnRole.Admin)]
public record AdminQuery() : IRequest<string>;

public class AdminQueryHandler : IRequestHandler<AdminQuery, string>
{
    public Task<string> Handle(AdminQuery request, CancellationToken cancellationToken) 
        => Task.FromResult("Only for Admins' Eyes");
}

