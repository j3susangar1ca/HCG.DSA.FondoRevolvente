using HCG.FondoRevolvente.Domain.Enums;

namespace HCG.FondoRevolvente.Domain.Interfaces;

/// <summary>
/// Interfaz para el servicio de máquina de estados de solicitudes.
/// Define el contrato para gestionar las 38 transiciones entre los 30 estados.
/// </summary>
public interface ISolicitudStateMachine
{
    /// <summary>
    /// Determina si una transición es válida desde el estado actual.
    /// </summary>
    /// <param name="estadoActual">Estado actual de la solicitud.</param>
    /// <param name="estadoDestino">Estado al que se desea transicionar.</param>
    /// <returns>True si la transición es válida.</returns>
    bool PuedeTransicionar(EstadoSolicitud estadoActual, EstadoSolicitud estadoDestino);

    /// <summary>
    /// Obtiene todos los estados posibles desde un estado dado.
    /// </summary>
    /// <param name="estadoActual">Estado actual.</param>
    /// <returns>Colección de estados a los que se puede transicionar.</returns>
    IEnumerable<EstadoSolicitud> ObtenerEstadosPosibles(EstadoSolicitud estadoActual);

    /// <summary>
    /// Obtiene todas las transiciones válidas desde un estado.
    /// </summary>
    /// <param name="estadoActual">Estado actual.</param>
    /// <returns>Colección de transiciones válidas.</returns>
    IEnumerable<TransicionEstado> ObtenerTransicionesValidas(EstadoSolicitud estadoActual);

    /// <summary>
    /// Valida una transición y devuelve el error si no es válida.
    /// </summary>
    /// <param name="estadoActual">Estado actual.</param>
    /// <param name="estadoDestino">Estado destino.</param>
    /// <param name="error">Mensaje de error si la transición no es válida.</param>
    /// <returns>True si la transición es válida.</returns>
    bool ValidarTransicion(EstadoSolicitud estadoActual, EstadoSolicitud estadoDestino, out string? error);

    /// <summary>
    /// Obtiene información detallada de una transición específica.
    /// </summary>
    TransicionEstado? ObtenerTransicion(EstadoSolicitud origen, EstadoSolicitud destino);

    /// <summary>
    /// Obtiene todas las transiciones definidas en el sistema.
    /// </summary>
    IReadOnlyList<TransicionEstado> ObtenerTodasLasTransiciones();
}

/// <summary>
/// Representa una transición entre dos estados.
/// </summary>
public sealed record TransicionEstado
{
    /// <summary>
    /// Estado origen de la transición.
    /// </summary>
    public EstadoSolicitud EstadoOrigen { get; init; }

    /// <summary>
    /// Estado destino de la transición.
    /// </summary>
    public EstadoSolicitud EstadoDestino { get; init; }

    /// <summary>
    /// Nombre de la acción que provoca la transición.
    /// </summary>
    public string Accion { get; init; } = null!;

    /// <summary>
    /// Descripción de la transición.
    /// </summary>
    public string Descripcion { get; init; } = null!;

    /// <summary>
    /// Rol(es) que pueden ejecutar esta transición.
    /// </summary>
    public RolAplicacion[] RolesPermitidos { get; init; } = [];

    /// <summary>
    /// Indica si la transición requiere comentario obligatorio.
    /// </summary>
    public bool RequiereComentario { get; init; }

    /// <summary>
    /// Indica si la transición es automática (ej: retry SAT).
    /// </summary>
    public bool EsAutomatica { get; init; }

    /// <summary>
    /// Condiciones adicionales para la transición.
    /// </summary>
    public string? Condiciones { get; init; }

    public TransicionEstado(
        EstadoSolicitud estadoOrigen,
        EstadoSolicitud estadoDestino,
        string accion,
        string descripcion,
        RolAplicacion[] rolesPermitidos,
        bool requiereComentario = false,
        bool esAutomatica = false,
        string? condiciones = null)
    {
        EstadoOrigen = estadoOrigen;
        EstadoDestino = estadoDestino;
        Accion = accion;
        Descripcion = descripcion;
        RolesPermitidos = rolesPermitidos;
        RequiereComentario = requiereComentario;
        EsAutomatica = esAutomatica;
        Condiciones = condiciones;
    }
}
