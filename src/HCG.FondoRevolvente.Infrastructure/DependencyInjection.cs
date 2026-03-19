using HCG.FondoRevolvente.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HCG.FondoRevolvente.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // En un escenario real, aquí se registrarían los servicios de SAT, SMB, SSRS, etc.
        // services.AddScoped<ISatValidationService, SatValidationService>();

        // Registro de servicios comunes
        services.AddScoped<IDateTimeService, DateTimeService>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }
}
