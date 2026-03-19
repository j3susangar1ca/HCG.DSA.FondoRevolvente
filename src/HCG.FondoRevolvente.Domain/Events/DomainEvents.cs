namespace HCG.FondoRevolvente.Domain.Events;

/// <summary>
/// Interfaz base para todos los eventos de dominio.
/// Los eventos de dominio son emitidos por los agregados para notificar
/// cambios significativos que pueden interesar a otros componentes del sistema.
/// </summary>
public interface IDomainEvent
{
    /// <summary>
    /// Fecha y hora en que ocurrió el evento.
    /// </summary>
    DateTime OcurridoEn { get; }
}

/// <summary>
/// Clase base para eventos de dominio con timestamp automático.
/// </summary>
public abstract record DomainEventBase : IDomainEvent
{
    public DateTime OcurridoEn { get; } = DateTime.UtcNow;
}

// ───────────────────────────────────────────────────────
// Eventos del ciclo de vida de Solicitud
// ───────────────────────────────────────────────────────

public record SolicitudCreadaEvent(int SolicitudId, string Folio, string? Usuario) : DomainEventBase;

public record SolicitudEnviadaCotizacionEvent(int SolicitudId, string Folio, string? Usuario) : DomainEventBase;

public record ProveedorSeleccionadoEvent(int SolicitudId, string Folio, int ProveedorId, int CotizacionId) : DomainEventBase;

public record SolicitudAutorizadaCaaEvent(int SolicitudId, string Folio, string? Usuario) : DomainEventBase;

public record SolicitudRechazadaCaaEvent(int SolicitudId, string Folio, string Motivo, string? Usuario) : DomainEventBase;

public record CfdiValidadoEvent(int SolicitudId, string Folio, string UuidCfdi) : DomainEventBase;

public record PagoRealizadoEvent(int SolicitudId, string Folio, decimal Monto) : DomainEventBase;

public record EntregaConfirmadaEvent(int SolicitudId, string Folio) : DomainEventBase;

public record SolicitudCerradaEvent(int SolicitudId, string Folio) : DomainEventBase;

public record SolicitudCanceladaEvent(int SolicitudId, string Folio, string Motivo, string? Usuario) : DomainEventBase;
