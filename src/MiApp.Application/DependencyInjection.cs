using FluentValidation;
using MediatR;
using MiApp.Application.Common.Behaviors;
using MiApp.Application.Features.Auth.Commands;
using Microsoft.Extensions.DependencyInjection;

namespace MiApp.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(LoginCommandHandler).Assembly);
            cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        });

        services.AddValidatorsFromAssembly(typeof(LoginCommandHandler).Assembly);

        return services;
    }
}
