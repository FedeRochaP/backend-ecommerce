using MiApp.Application.DTOs;
using MiApp.Application.Interfaces;
using MiApp.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace MiApp.WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly LoginUseCase _loginUseCase;
    private readonly RegisterUseCase _registerUseCase;

    public AuthController(LoginUseCase loginUseCase, RegisterUseCase registerUseCase)
    {
        _loginUseCase = loginUseCase;
        _registerUseCase = registerUseCase;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        var response = await _loginUseCase.ExecuteAsync(request, ct);
        if (response is null)
            return Unauthorized(new { message = "Credenciales inválidas." });

        return Ok(response);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        var user = await _registerUseCase.ExecuteAsync(request, ct);
        if (user is null)
            return Conflict(new { message = "El email ya está registrado." });

        return Created($"/api/auth/{user.Id}", new { user.Id, user.Email, user.Name });
    }
}
