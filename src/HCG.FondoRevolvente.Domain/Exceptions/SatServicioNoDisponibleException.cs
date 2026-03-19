namespace HCG.FondoRevolvente.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando el servicio del SAT no está disponible.
/// Implementa RN-004: Resiliencia con Polly para fallos del servicio externo.
/// </summary>
public class SatServicioNoDisponibleException : DomainException
{
    /// <summary>
    /// Número de intentos realizados antes de fallar.
    /// </summary>
    public int IntentosRealizados { get; }

    /// <summary>
    /// Tiempo sugerido para el próximo reintento.
    /// </summary>
    public TimeSpan TiempoReintentoSugerido { get; }

    public SatServicioNoDisponibleException(int intentosRealizados, Exception? innerException = null)
        : base(
            "RN004_SAT_NO_DISPONIBLE",
            $"El servicio de validación del SAT no está disponible después de {intentosRealizados} intentos. " +
            $"Se activó el Circuit Breaker. Intente nuevamente en unos minutos.",
            innerException)
    {
        IntentosRealizados = intentosRealizados;
        TiempoReintentoSugerido = TimeSpan.FromMinutes(Constants.LimitesNegocio.MinutosCircuitoAbierto);

        ConDatos("IntentosRealizados", intentosRealizados)
            .ConDatos("TiempoReintentoSugeridoMinutos", Constants.LimitesNegocio.MinutosCircuitoAbierto);
    }
}
