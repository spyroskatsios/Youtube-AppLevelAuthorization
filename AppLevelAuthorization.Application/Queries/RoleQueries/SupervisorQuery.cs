using MediatR;

namespace AppLevelAuthorization.Application.Queries.RoleQueries;

public record SupervisorQuery() : IRequest<string>;

public class SupervisorQueryHandler : IRequestHandler<SupervisorQuery, string>
{
    public Task<string> Handle(SupervisorQuery request, CancellationToken cancellationToken) 
        => Task.FromResult("Hello Supervisor");
}