using HCG.FondoRevolvente.Domain.Constants;

namespace HCG.FondoRevolvente.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando no se cuenta con el número mínimo de cotizaciones requeridas.
/// Implementa RN-003: Mínimo 3 cotizaciones por operación.
/// </summary>
public class CotizacionesInsuficientesException : DomainException
{
    /// <summary>
    /// Número de cotizaciones recibidas actualmente.
    /// </summary>
    public int CotizacionesRecibidas { get; }

    /// <summary>
    /// Número mínimo de cotizaciones requeridas.
    /// </summary>
    public int CotizacionesRequeridas { get; }

    /// <summary>
    /// Número de cotizaciones faltantes.
    /// </summary>
    public int CotizacionesFaltantes { get; }

    public CotizacionesInsuficientesException(int recibidas, int requeridas = LimitesNegocio.CotizacionesMinimasRequeridas)
        : base(
            "RN003_COTIZACIONES_INSUFICIENTES",
            $"No se cuenta con el número mínimo de cotizaciones. " +
            $"Recibidas: {recibidas}, Requeridas: {requeridas}. " +
            $"Faltan {requeridas - recibidas} cotizaciones para continuar.")
    {
        CotizacionesRecibidas = recibidas;
        CotizacionesRequeridas = requeridas;
        CotizacionesFaltantes = requeridas - recibidas;

        ConDatos("CotizacionesRecibidas", recibidas)
            .ConDatos("CotizacionesRequeridas", requeridas)
            .ConDatos("CotizacionesFaltantes", CotizacionesFaltantes);
    }

    public CotizacionesInsuficientesException(int recibidas, decimal montoSolicitud)
        : base(
            "RN003_COTIZACIONES_INSUFICIENTES",
            $"No se cuenta con el número mínimo de cotizaciones para la solicitud de {montoSolicitud:C2} MXN. " +
            $"Recibidas: {recibidas}, Requeridas: {Constants.CotizacionesRequeridas.ObtenerCotizacionesRequeridas(montoSolicitud)}.")
    {
        CotizacionesRecibidas = recibidas;
        CotizacionesRequeridas = Constants.CotizacionesRequeridas.ObtenerCotizacionesRequeridas(montoSolicitud);
        CotizacionesFaltantes = CotizacionesRequeridas - recibidas;

        ConDatos("CotizacionesRecibidas", recibidas)
            .ConDatos("CotizacionesRequeridas", CotizacionesRequeridas)
            .ConDatos("CotizacionesFaltantes", CotizacionesFaltantes)
            .ConDatos("MontoSolicitud", montoSolicitud);
    }
}
