namespace HCG.FondoRevolvente.Domain.Events;

/// <summary>
/// Interfaz base para todos los eventos de dominio.
/// Los eventos de dominio representan hechos significativos que ocurrieron en el dominio
/// y que pueden ser manejados por uno o más event handlers.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Fecha y hora en que ocurrió el evento.
    /// </summary>
    DateTime OccurredOn { get; }
}

/// <summary>
/// Clase base para eventos de dominio.
/// </summary>
public abstract record DomainEventBase : IDomainEvent
{
    public DateTime OccurredOn { get; } = DateTime.UtcNow;
}
