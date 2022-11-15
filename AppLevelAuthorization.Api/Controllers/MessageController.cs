using AppLevelAuthorization.Application;
using AppLevelAuthorization.Application.Queries.RoleQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;

namespace AppLevelAuthorization.Api.Controllers;

[ApiController]
[Authorize]
public class MessageController : ControllerBase
{
    private readonly IMediator _mediator;

    public MessageController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    
    [HttpGet(Routes.Messages.User)]
    public async Task<IActionResult> GetMessageForUser()
    {
        return Ok(await _mediator.Send(new UserQuery()));
    }
    
    [Authorize(Roles = "Supervisor")]
    [HttpGet(Routes.Messages.Supervisor)]
    public async Task<IActionResult> GetMessageForSupervisor()
    {
        return Ok(await _mediator.Send(new SupervisorQuery()));
    }

    [Authorize(Roles = "Manager")]
    [HttpGet(Routes.Messages.Manager)]
    public async Task<IActionResult> GetMessageForManager()
    {
        return Ok(await _mediator.Send(new ManagerQuery()));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet(Routes.Messages.Admin)]
    public async Task<IActionResult> GetMessageForAdmin()
    {
        return Ok(await _mediator.Send(new AdminQuery()));
    }
}