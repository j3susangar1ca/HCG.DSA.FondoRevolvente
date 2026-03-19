using HCG.FondoRevolvente.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace HCG.FondoRevolvente.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Solicitud> Solicitudes { get; }
    DbSet<Proveedor> Proveedores { get; }
    DbSet<Cotizacion> Cotizaciones { get; }
    DbSet<Hito> Historial { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
