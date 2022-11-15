using AppLevelAuthorization.Application.Interfaces;
using AppLevelAuthorization.Application.ServiceResults;
using MediatR;

namespace AppLevelAuthorization.Application.Commands.Auth;

public record SignInCommand(string Email, string Password) : IRequest<AuthResult>;

public class SignInHandler : IRequestHandler<SignInCommand, AuthResult>
{
    private readonly IIdentityService _identityService;

    public SignInHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<AuthResult> Handle(SignInCommand request, CancellationToken cancellationToken)
        => await _identityService.SignInUserAsync(request.Email, request.Password);
}