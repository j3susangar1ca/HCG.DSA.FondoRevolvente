using HCG.FondoRevolvente.Domain.Constants;
using HCG.FondoRevolvente.Domain.Exceptions;
using HCG.FondoRevolvente.Domain.ValueObjects;

namespace HCG.FondoRevolvente.Domain.Services;

/// <summary>
/// Servicio de dominio para detectar fraccionamiento de operaciones.
/// Implementa RN-002: Detección de fraccionamiento para evadir el límite de $75,000 MXN.
/// </summary>
public class ValidadorFraccionamientoService
{
    /// <summary>
    /// Analiza si una nueva solicitud al mismo proveedor constituye fraccionamiento.
    /// </summary>
    /// <param name="nuevoMonto">Monto de la nueva solicitud.</param>
    /// <param name="rfcProveedor">RFC del proveedor.</param>
    /// <param name="solicitudesPrevias">Solicitudes previas al mismo proveedor en el período.</param>
    /// <returns>Resultado del análisis de fraccionamiento.</returns>
    public ResultadoAnalisisFraccionamiento Analizar(
        decimal nuevoMonto,
        RfcProveedor rfcProveedor,
        IEnumerable<SolicitudPreviaProveedor> solicitudesPrevias)
    {
        var listaPrevias = solicitudesPrevias.ToList();
        var sumaPrevia = listaPrevias.Sum(s => s.Monto);
        var totalConNuevo = sumaPrevia + nuevoMonto;
        
        // El porcentaje se calcula sobre el límite máximo permitido
        var porcentajeLimite = (double)(totalConNuevo / LimitesNegocio.MontoMaximoFondoRevolvente);

        var resultado = new ResultadoAnalisisFraccionamiento
        {
            RfcProveedor = rfcProveedor,
            MontoNuevo = nuevoMonto,
            MontoPreviaSuma = sumaPrevia,
            MontoTotal = totalConNuevo,
            PorcentajeDelLimite = porcentajeLimite,
            SolicitudesPrevias = listaPrevias.AsReadOnly(),
            NumeroSolicitudesPrevias = listaPrevias.Count
        };

        // Determinar el nivel de alerta según los límites de negocio
        if (porcentajeLimite >= LimitesNegocio.PorcentajeFraccionamientoConfirmado)
        {
            resultado.EsFraccionamientoConfirmado = true;
            resultado.NivelAlerta = NivelAlertaFraccionamiento.Critico;
            resultado.Mensaje = $"FRACCIONAMIENTO DETECTADO: El proveedor {rfcProveedor.Enmascarado()} tiene " +
                               $"{listaPrevias.Count + 1} solicitudes por un total de {totalConNuevo:C2} MXN " +
                               $"({porcentajeLimite:P2} del límite normativo).";
        }
        else if (porcentajeLimite >= LimitesNegocio.PorcentajeAlertaFraccionamiento)
        {
            resultado.EsAlerta = true;
            resultado.NivelAlerta = NivelAlertaFraccionamiento.Advertencia;
            resultado.Mensaje = $"ALERTA: Posible fraccionamiento. El proveedor {rfcProveedor.Enmascarado()} tiene " +
                               $"{listaPrevias.Count + 1} solicitudes por un total de {totalConNuevo:C2} MXN " +
                               $"({porcentajeLimite:P2} del límite). Se requiere revisión.";
        }
        else
        {
            resultado.NivelAlerta = NivelAlertaFraccionamiento.Ninguno;
            resultado.Mensaje = $"El proveedor {rfcProveedor.Enmascarado()} tiene {listaPrevias.Count} solicitudes " +
                               $"previas por {sumaPrevia:C2} MXN. Total con nueva solicitud: {totalConNuevo:C2} MXN " +
                               $"({porcentajeLimite:P2} del límite).";
        }

        return resultado;
    }

    /// <summary>
    /// Valida si se puede proceder con la solicitud o si debe bloquearse por fraccionamiento.
    /// </summary>
    /// <param name="nuevoMonto">Monto de la nueva solicitud.</param>
    /// <param name="rfcProveedor">RFC del proveedor.</param>
    /// <param name="solicitudesPrevias">Solicitudes previas al mismo proveedor.</param>
    /// <exception cref="FraccionamientoDetectadoException">
    /// Lanzada cuando se detecta fraccionamiento confirmado.
    /// </exception>
    public void ValidarOViolacion(
        decimal nuevoMonto,
        RfcProveedor rfcProveedor,
        IEnumerable<SolicitudPreviaProveedor> solicitudesPrevias)
    {
        var resultado = Analizar(nuevoMonto, rfcProveedor, solicitudesPrevias);

        if (resultado.EsFraccionamientoConfirmado)
        {
            throw new FraccionamientoDetectadoException(
                rfcProveedor.Valor,
                resultado.MontoTotal,
                resultado.NumeroSolicitudesPrevias + 1,
                LimitesNegocio.MesesDeteccionFraccionamiento * 30, // aproximación en días
                resultado.SolicitudesPrevias.Select(s => s.Folio));
        }
    }
}

/// <summary>
/// Información de una solicitud previa al mismo proveedor.
/// </summary>
public sealed record SolicitudPreviaProveedor
{
    /// <summary>
    /// Folio de la solicitud previa.
    /// </summary>
    public string Folio { get; init; } = null!;

    /// <summary>
    /// Monto de la solicitud previa.
    /// </summary>
    public decimal Monto { get; init; }

    /// <summary>
    /// Fecha de la solicitud previa.
    /// </summary>
    public DateTime Fecha { get; init; }

    /// <summary>
    /// Estado actual de la solicitud previa.
    /// </summary>
    public string Estado { get; init; } = null!;
}

/// <summary>
/// Resultado del análisis de fraccionamiento.
/// </summary>
public sealed class ResultadoAnalisisFraccionamiento
{
    /// <summary>
    /// RFC del proveedor analizado.
    /// </summary>
    public RfcProveedor? RfcProveedor { get; init; }

    /// <summary>
    /// Monto de la nueva solicitud.
    /// </summary>
    public decimal MontoNuevo { get; init; }

    /// <summary>
    /// Suma de montos de solicitudes previas.
    /// </summary>
    public decimal MontoPreviaSuma { get; init; }

    /// <summary>
    /// Total incluyendo la nueva solicitud.
    /// </summary>
    public decimal MontoTotal { get; init; }

    /// <summary>
    /// Porcentaje del límite normativo que representa el total.
    /// </summary>
    public double PorcentajeDelLimite { get; init; }

    /// <summary>
    /// Solicitudes previas encontradas.
    /// </summary>
    public IReadOnlyList<SolicitudPreviaProveedor> SolicitudesPrevias { get; init; } = [];

    /// <summary>
    /// Número de solicitudes previas encontradas.
    /// </summary>
    public int NumeroSolicitudesPrevias { get; init; }

    /// <summary>
    /// Indica si se detectó fraccionamiento confirmado.
    /// </summary>
    public bool EsFraccionamientoConfirmado { get; set; }

    /// <summary>
    /// Indica si se generó una alerta de posible fraccionamiento.
    /// </summary>
    public bool EsAlerta { get; set; }

    /// <summary>
    /// Nivel de alerta del análisis.
    /// </summary>
    public NivelAlertaFraccionamiento NivelAlerta { get; set; }

    /// <summary>
    /// Mensaje descriptivo del resultado.
    /// </summary>
    public string Mensaje { get; set; } = string.Empty;
}

/// <summary>
/// Niveles de alerta para fraccionamiento.
/// </summary>
public enum NivelAlertaFraccionamiento
{
    /// <summary>
    /// Sin alerta. El total está dentro de parámetros normales.
    /// </summary>
    Ninguno = 0,

    /// <summary>
    /// Advertencia. El total supera el 80% del límite.
    /// </summary>
    Advertencia = 1,

    /// <summary>
    /// Crítico. Se detectó fraccionamiento confirmado.
    /// </summary>
    Critico = 2
}
