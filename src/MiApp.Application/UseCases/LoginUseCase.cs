using MiApp.Application.DTOs;
using MiApp.Application.Interfaces;

namespace MiApp.Application.UseCases;

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginUseCase(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        ITokenService tokenService)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<LoginResponse?> ExecuteAsync(LoginRequest request, CancellationToken ct = default)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, ct);
        if (user is null) return null;

        if (!_passwordHasher.Verify(request.Password, user.PasswordHash)) return null;

        var token = _tokenService.GenerateToken(user);
        return new LoginResponse(user.Id, token);
    }
}
