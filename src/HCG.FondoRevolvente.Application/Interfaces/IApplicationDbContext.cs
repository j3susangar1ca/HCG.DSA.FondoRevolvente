using HCG.FondoRevolvente.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace HCG.FondoRevolvente.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Solicitud> Solicitudes { get; }
    DbSet<Proveedor> Proveedores { get; }
    DbSet<Cotizacion> Cotizaciones { get; }
    DbSet<Hito> Historial { get; }

    /// <summary>
    /// Transacción activa actual, null si no hay transacción.
    /// </summary>
    IDbContextTransaction? CurrentTransaction { get; }

    /// <summary>
    /// Inicia una nueva transacción de base de datos.
    /// </summary>
    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
