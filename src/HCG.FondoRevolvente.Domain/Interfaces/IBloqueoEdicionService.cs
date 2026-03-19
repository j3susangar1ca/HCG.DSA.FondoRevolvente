namespace HCG.FondoRevolvente.Domain.Interfaces;

/// <summary>
/// Interfaz para el servicio de bloqueo de edición.
/// Implementa RN-005: Bloqueo optimista de edición.
/// </summary>
public interface IBloqueoEdicionService
{
    /// <summary>
    /// Intenta adquirir el bloqueo de edición para una solicitud.
    /// </summary>
    /// <param name="solicitudId">ID de la solicitud.</param>
    /// <param name="usuario">Usuario que solicita el bloqueo.</param>
    /// <param name="nombreCompleto">Nombre completo del usuario.</param>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>True si el bloqueo fue adquirido exitosamente.</returns>
    Task<bool> AdquirirBloqueoAsync(
        int solicitudId,
        string usuario,
        string nombreCompleto,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Libera el bloqueo de edición de una solicitud.
    /// </summary>
    Task LiberarBloqueoAsync(int solicitudId, string usuario, CancellationToken cancellationToken = default);

    /// <summary>
    /// Renueva el bloqueo de edición si el usuario lo tiene.
    /// </summary>
    Task<bool> RenovarBloqueoAsync(int solicitudId, string usuario, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica si una solicitud está bloqueada.
    /// </summary>
    Task<bool> EstaBloqueadaAsync(int solicitudId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtiene información del bloqueo actual.
    /// </summary>
    Task<InfoBloqueo?> ObtenerInfoBloqueoAsync(int solicitudId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Información sobre un bloqueo de edición.
/// </summary>
public sealed record InfoBloqueo
{
    public string Usuario { get; init; } = null!;
    public string NombreCompleto { get; init; } = null!;
    public DateTime FechaAdquisicion { get; init; }
    public TimeSpan TiempoRestante { get; init; }
    public bool EstaExpirado { get; init; }
}
