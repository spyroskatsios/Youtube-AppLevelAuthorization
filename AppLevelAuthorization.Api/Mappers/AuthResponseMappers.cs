using AppLevelAuthorization.Api.Responses;
using AppLevelAuthorization.Application.ServiceResults;

namespace AppLevelAuthorization.Api.Mappers;

public static class AuthResponseMappers
{
    public static AuthResponse ToAuthResponse(this AuthResult result)
        => new()
        {
            Success = result.Success,
            Token = result.Token,
            Errors = result.Errors
        };
}