using MediatR;
using MiApp.Application.Features.Auth.Commands;
using Microsoft.AspNetCore.Mvc;

namespace MiApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ISender _sender;

    public AuthController(ISender sender) => _sender = sender;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken ct)
    {
        var response = await _sender.Send(command, ct);
        if (response is null)
            return Unauthorized(new { message = "Credenciales inválidas." });

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command, CancellationToken ct)
    {
        var response = await _sender.Send(command, ct);
        if (response is null)
            return Conflict(new { message = "El email ya está registrado." });

        return Created($"/api/auth/{response.Id}", response);
    }
}
