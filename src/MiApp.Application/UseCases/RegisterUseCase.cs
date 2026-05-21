using MiApp.Application.DTOs;
using MiApp.Application.Interfaces;
using MiApp.Domain.Entities;

namespace MiApp.Application.UseCases;

public class RegisterUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterUseCase(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Registra un nuevo usuario. Devuelve null si el email ya está en uso.
    /// </summary>
    public async Task<User?> ExecuteAsync(RegisterRequest request, CancellationToken ct = default)
    {
        var existing = await _userRepository.GetByEmailAsync(request.Email, ct);
        if (existing is not null) return null;

        var user = new User(
            Guid.NewGuid(),
            request.Email,
            request.Name,
            _passwordHasher.Hash(request.Password));

        await _userRepository.AddAsync(user, ct);
        return user;
    }
}
