using MediatR;
using MiApp.Application.Common;
using MiApp.Application.Interfaces;
using MiApp.Domain.Entities;

namespace MiApp.Application.Features.Auth.Commands;

public record RegisterCommand(string Email, string Name, string Password) : ICommand<RegisterResponse?>;

public record RegisterResponse(Guid Id, string Email, string Name);

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RegisterResponse?>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<RegisterResponse?> Handle(RegisterCommand command, CancellationToken ct)
    {
        var existing = await _userRepository.GetByEmailAsync(command.Email, ct);
        if (existing is not null) return null;

        var user = new User(
            Guid.NewGuid(),
            command.Email,
            command.Name,
            _passwordHasher.Hash(command.Password));

        await _userRepository.AddAsync(user, ct);
        return new RegisterResponse(user.Id, user.Email, user.Name);
    }
}
