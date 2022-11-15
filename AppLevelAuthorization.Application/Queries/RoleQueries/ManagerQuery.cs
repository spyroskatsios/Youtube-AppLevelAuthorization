using MediatR;

namespace AppLevelAuthorization.Application.Queries.RoleQueries;

public record ManagerQuery() : IRequest<string>;

public class ManagerQueryHandler : IRequestHandler<ManagerQuery, string>
{
    public Task<string> Handle(ManagerQuery request, CancellationToken cancellationToken) 
        => Task.FromResult("Hello Manager");
}