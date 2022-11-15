using AppLevelAuthorization.Api.Mappers;
using AppLevelAuthorization.Api.Requests;
using AppLevelAuthorization.Api.Responses;
using AppLevelAuthorization.Application.Commands.Auth;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppLevelAuthorization.Api.Controllers;

[ApiController]
public class IdentityController : ControllerBase
{
    private readonly IMediator _mediator;

    public IdentityController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    private IActionResult OkOrBadRequest(AuthResponse response)
        => response.Success ? Ok(response) : BadRequest(response);
    
    
    [HttpPost(Routes.Auth.SignIn)]
    public async Task<IActionResult> SignIn([FromBody] SignInRequest request)
    {
        var result = await _mediator.Send(new SignInCommand(request.Email, request.Password));
        return OkOrBadRequest(result.ToAuthResponse());
    }
}