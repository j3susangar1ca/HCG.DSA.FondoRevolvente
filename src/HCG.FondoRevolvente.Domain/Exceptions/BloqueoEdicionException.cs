using HCG.FondoRevolvente.Domain.Constants;

namespace HCG.FondoRevolvente.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando un usuario intenta editar un recurso bloqueado por otro usuario.
/// Implementa RN-005: Bloqueo optimista de edición para prevenir conflictos de concurrencia.
/// </summary>
public class BloqueoEdicionException : DomainException
{
    /// <summary>
    /// Identificador del recurso bloqueado (ej: ID de solicitud).
    /// </summary>
    public int RecursoId { get; }

    /// <summary>
    /// Tipo del recurso bloqueado.
    /// </summary>
    public string TipoRecurso { get; }

    /// <summary>
    /// Nombre del usuario que tiene el bloqueo activo.
    /// </summary>
    public string UsuarioBloqueo { get; }

    /// <summary>
    /// Fecha y hora cuando se adquirió el bloqueo.
    /// </summary>
    public DateTime FechaAdquisicion { get; }

    /// <summary>
    /// Tiempo restante antes de que expire el bloqueo.
    /// </summary>
    public TimeSpan TiempoRestante { get; }

    public BloqueoEdicionException(
        int recursoId,
        string tipoRecurso,
        string usuarioBloqueo,
        DateTime fechaAdquisicion,
        DateTime fechaActual)
        : base(
            "RN005_BLOQUEO_EDICION",
            $"El {tipoRecurso} con ID {recursoId} está siendo editado por {usuarioBloqueo} " +
            $"desde las {fechaAdquisicion:HH:mm}. " +
            $"El bloqueo expira en {FormatTiempoRestante(ConfiguracionBloqueo.TiempoRestante(fechaAdquisicion, fechaActual))}.")
    {
        RecursoId = recursoId;
        TipoRecurso = tipoRecurso;
        UsuarioBloqueo = usuarioBloqueo;
        FechaAdquisicion = fechaAdquisicion;
        TiempoRestante = ConfiguracionBloqueo.TiempoRestante(fechaAdquisicion, fechaActual);

        ConDatos("RecursoId", recursoId)
            .ConDatos("TipoRecurso", tipoRecurso)
            .ConDatos("UsuarioBloqueo", usuarioBloqueo)
            .ConDatos("FechaAdquisicion", fechaAdquisicion)
            .ConDatos("TiempoRestanteMinutos", TiempoRestante.TotalMinutes);
    }

    /// <summary>
    /// Formatea el tiempo restante en formato legible.
    /// </summary>
    private static string FormatTiempoRestante(TimeSpan tiempo) =>
        tiempo.TotalMinutes >= 1
            ? $"{(int)tiempo.TotalMinutes} minuto(s) y {tiempo.Seconds} segundos"
            : $"{tiempo.Seconds} segundos";
}
