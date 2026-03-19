using HCG.FondoRevolvente.Domain.Interfaces;
using HCG.FondoRevolvente.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace HCG.FondoRevolvente.Domain;

public static class DependencyInjection
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddSingleton<IBloqueoEdicionService, BloqueoEdicionService>();
        services.AddScoped<ISolicitudStateMachine, SolicitudStateMachine>();
        services.AddScoped<ValidadorFraccionamientoService>();
        
        return services;
    }
}
