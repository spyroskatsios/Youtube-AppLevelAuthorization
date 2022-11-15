using MediatR;

namespace AppLevelAuthorization.Application.Queries.RoleQueries;

public record UserQuery() : IRequest<string>;

public class UserQueryHandler : IRequestHandler<UserQuery, string>
{
    public Task<string> Handle(UserQuery request, CancellationToken cancellationToken) 
        => Task.FromResult("Hello User");
}