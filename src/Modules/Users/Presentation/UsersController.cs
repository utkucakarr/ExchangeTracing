using ExchangeTracing.Modules.Users.Application;
using ExchangeTracing.Modules.Users.Application.CreateUser;
using ExchangeTracing.Modules.Users.Application.GetUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeTracing.Modules.Users.Presentation;

[ApiController]
[Route("users")]
public sealed class UsersController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<UserDto>> Create(
        CreateUserCommand command,
        CancellationToken cancellationToken)
    {
        var user = await sender.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<UserDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var user = await sender.Send(new GetUserQuery(id), cancellationToken);
        return user is null ? NotFound() : Ok(user);
    }
}
