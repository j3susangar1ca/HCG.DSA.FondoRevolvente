using HCG.FondoRevolvente.Application.Interfaces;
using HCG.FondoRevolvente.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;

namespace HCG.FondoRevolvente.Persistence;

public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Solicitud> Solicitudes => Set<Solicitud>();
    public DbSet<Proveedor> Proveedores => Set<Proveedor>();
    public DbSet<Cotizacion> Cotizaciones => Set<Cotizacion>();
    public DbSet<Hito> Historial => Set<Hito>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);

        // Configuraciones manuales si no se usan clases de configuración
        modelBuilder.Entity<Solicitud>(entity =>
        {
            entity.OwnsOne(s => s.Folio);
            entity.OwnsOne(s => s.Monto);
        });

        modelBuilder.Entity<Proveedor>(entity =>
        {
            entity.OwnsOne(p => p.Rfc);
        });

        modelBuilder.Entity<Cotizacion>(entity =>
        {
            entity.OwnsOne(c => c.MontoSubtotal);
            entity.OwnsOne(c => c.MontoTotal);
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
