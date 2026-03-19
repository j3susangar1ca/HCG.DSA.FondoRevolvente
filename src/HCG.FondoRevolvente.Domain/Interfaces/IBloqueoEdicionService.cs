namespace HCG.FondoRevolvente.Domain.Interfaces;

/// <summary>
/// Contrato para la gestión de bloqueos de edición concurrente (Pessimistic Locking).
/// §Módulo 06 y RN-005.
/// </summary>
public interface IBloqueoEdicionService
{
    /// <summary>
    /// Intenta adquirir un bloqueo para el usuario en el expediente indicado.
    /// </summary>
    bool TryAcquireLock(string folio, string usuario, out string? poseedorActual);

    /// <summary>
    /// Libera el bloqueo del expediente si el usuario es quien lo posee.
    /// </summary>
    void ReleaseLock(string folio, string usuario);

    /// <summary>
    /// Verifica si un expediente está actualmente bloqueado por alguien que no sea el usuario.
    /// </summary>
    bool IsLockedByOther(string folio, string usuario, out string? poseedorActual);
}
