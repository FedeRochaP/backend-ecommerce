using MediatR;
using MiApp.Application.Common;
using MiApp.Application.DTOs;
using MiApp.Application.Interfaces;

namespace MiApp.Application.Features.Auth.Commands;

public record LoginCommand(string Email, string Password) : ICommand<LoginResponse?>;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponse?>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse?> Handle(LoginCommand command, CancellationToken ct)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email, ct);
        if (user is null) return null;

        if (!_passwordHasher.Verify(command.Password, user.PasswordHash)) return null;

        var token = _tokenService.GenerateToken(user);
        return new LoginResponse(user.Id, token);
    }
}
