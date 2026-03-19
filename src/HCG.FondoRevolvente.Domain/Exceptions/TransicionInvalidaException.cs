namespace HCG.FondoRevolvente.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando una transición de estado no es válida.
/// </summary>
public class TransicionInvalidaException : DomainException
{
    /// <summary>
    /// Estado actual de la solicitud.
    /// </summary>
    public Enums.EstadoSolicitud EstadoActual { get; }

    /// <summary>
    /// Estado al que se intentó transicionar.
    /// </summary>
    public Enums.EstadoSolicitud EstadoDestino { get; }

    /// <summary>
    /// Razón por la cual la transición no es válida.
    /// </summary>
    public string Razon { get; }

    public TransicionInvalidaException(
        Enums.EstadoSolicitud estadoActual,
        Enums.EstadoSolicitud estadoDestino,
        string razon)
        : base(
            "TRANSICION_INVALIDA",
            $"No es posible transicionar de '{estadoActual}' a '{estadoDestino}'. {razon}")
    {
        EstadoActual = estadoActual;
        EstadoDestino = estadoDestino;
        Razon = razon;

        ConDatos("EstadoActual", estadoActual.ToString())
            .ConDatos("EstadoDestino", estadoDestino.ToString())
            .ConDatos("Razon", razon);
    }
}
