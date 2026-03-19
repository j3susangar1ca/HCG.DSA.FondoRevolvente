using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace HCG.FondoRevolvente.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddAutoMapper(assembly);
        services.AddValidatorsFromAssembly(assembly);
        services.AddMediatR(cfg => {
            cfg.RegisterServicesFromAssembly(assembly);
            // Aqui se pueden agregar Behaviors (Validation, Logging, etc.)
        });

        return services;
    }
}
