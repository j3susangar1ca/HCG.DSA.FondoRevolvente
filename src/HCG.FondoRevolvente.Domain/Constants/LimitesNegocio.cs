namespace HCG.FondoRevolvente.Domain.Constants;

/// <summary>
/// Constantes que definen los límites y restricciones del negocio.
/// Estos valores están alineados con la normativa del Estado de Jalisco.
/// Referencia: Ley de Compras del Estado de Jalisco, Art. 57 del Reglamento.
/// </summary>
public static class LimitesNegocio
{
    #region Límites Monetarios (RN-001)

    /// <summary>
    /// Monto máximo permitido por operación de Fondo Revolvente.
    /// Establecido por la Ley de Compras del Estado de Jalisco, Art. 57.
    /// Valor: $75,000.00 MXN
    /// </summary>
    public const decimal MontoMaximoFondoRevolvente = 75_000.00m;

    /// <summary>
    /// Monto mínimo permitido para una solicitud de adquisición.
    /// Valor: $100.00 MXN
    /// </summary>
    public const decimal MontoMinimoSolicitud = 100.00m;

    /// <summary>
    /// Tasa de IVA estándar en México (16%).
    /// </summary>
    public const decimal TasaIvaEstandar = 0.16m;

    /// <summary>
    /// Tasa de IVA en zona fronteriza (8%).
    /// </summary>
    public const decimal TasaIvaZonaFronteriza = 0.08m;

    /// <summary>Umbral para alerta amarilla en la barra de progreso del MontoDisplay (70%).</summary>
    public const double UmbralAlertaAmarillaMonto = 0.70;

    /// <summary>Umbral para alerta roja en la barra de progreso del MontoDisplay (90%).</summary>
    public const double UmbralAlertaRojaMonto = 0.90;

    #endregion

    #region Detección de Fraccionamiento (RN-002)

    /// <summary>
    /// Número de meses hacia atrás para detectar fraccionamiento.
    /// El sistema busca solicitudes del mismo proveedor en este período.
    /// Valor: 3 meses
    /// </summary>
    public const int MesesDeteccionFraccionamiento = 3;

    /// <summary>
    /// Porcentaje del límite que activa alerta de posible fraccionamiento.
    /// Si la suma de solicitudes al mismo proveedor en el período excede este porcentaje del límite,
    /// se genera una alerta para revisión.
    /// Valor: 80% (0.80)
    /// </summary>
    public const double PorcentajeAlertaFraccionamiento = 0.80;

    /// <summary>
    /// Porcentaje del límite que indica fraccionamiento confirmado.
    /// Si la suma de solicitudes al mismo proveedor en el período excede este porcentaje del límite,
    /// se rechaza automáticamente la nueva solicitud.
    /// Valor: 100% (1.00)
    /// </summary>
    public const double PorcentajeFraccionamientoConfirmado = 1.00;

    #endregion

    #region Cotizaciones (RN-003)

    /// <summary>
    /// Número mínimo de cotizaciones requeridas por normativa.
    /// Valor: 3 cotizaciones
    /// </summary>
    public const int CotizacionesMinimasRequeridas = 3;

    /// <summary>
    /// Número máximo de cotizaciones que se pueden adjuntar a una solicitud.
    /// Valor: 10 cotizaciones
    /// </summary>
    public const int CotizacionesMaximasPermitidas = 10;

    /// <summary>
    /// Días máximos para recibir cotizaciones antes de considerar el proceso vencido.
    /// Valor: 15 días naturales
    /// </summary>
    public const int DiasMaximosCotizacion = 15;

    #endregion

    #region Validación SAT (RN-004)

    /// <summary>
    /// Número máximo de reintentos para validación CFDI ante el SAT.
    /// Valor: 3 reintentos
    /// </summary>
    public const int MaximoReintentosValidacionSat = 3;

    /// <summary>
    /// Segundos de espera entre reintentos de validación SAT.
    /// Valor: 30 segundos
    /// </summary>
    public const int SegundosEntreReintentosSat = 30;

    /// <summary>
    /// Segundos de timeout para llamadas al servicio del SAT.
    /// Valor: 60 segundos
    /// </summary>
    public const int TimeoutSegundosSat = 60;

    /// <summary>
    /// Número de fallas consecutivas para abrir el circuito (Circuit Breaker).
    /// Valor: 5 fallas
    /// </summary>
    public const int FallasParaCircuitBreaker = 5;

    /// <summary>
    /// Minutos que el circuito permanece abierto antes de intentar medio-abrir.
    /// Valor: 5 minutos
    /// </summary>
    public const int MinutosCircuitoAbierto = 5;

    #endregion

    #region Bloqueo de Edición (RN-005)

    /// <summary>
    /// Minutos de duración del bloqueo de edición antes de expirar.
    /// Valor: 30 minutos
    /// </summary>
    public const int MinutosDuracionBloqueo = 30;

    /// <summary>
    /// Minutos antes de la expiración para renovar automáticamente el bloqueo.
    /// El cliente renueva el bloqueo cuando faltan estos minutos para expirar.
    /// Valor: 5 minutos
    /// </summary>
    public const int MinutosRenovacionBloqueo = 5;

    #endregion

    #region Expediente y Archivos

    /// <summary>
    /// Tamaño máximo de archivo PDF para documentos del expediente (en bytes).
    /// Valor: 10 MB (10,485,760 bytes)
    /// </summary>
    public const long TamanoMaximoArchivoPdfBytes = 10 * 1024 * 1024;

    /// <summary>
    /// Tamaño máximo de archivo XML para CFDI (en bytes).
    /// Valor: 1 MB (1,048,576 bytes)
    /// </summary>
    public const long TamanoMaximoArchivoXmlBytes = 1024 * 1024;

    /// <summary>
    /// Extensiones de archivo permitidas para documentos del expediente.
    /// </summary>
    public static readonly string[] ExtensionesPermitidasDocumentos = [".pdf", ".xml"];

    /// <summary>
    /// Extensiones de archivo permitidas para cotizaciones.
    /// </summary>
    public static readonly string[] ExtensionesPermitidasCotizaciones = [".pdf", ".xlsx", ".xls", ".docx", ".doc"];

    #endregion

    #region Tiempos de Proceso

    /// <summary>
    /// Días máximos para que el CAA responda una solicitud en autorización.
    /// Valor: 5 días hábiles
    /// </summary>
    public const int DiasHabilesRespuestaCaa = 5;

    /// <summary>
    /// Días máximos para que Finanzas procese un pago.
    /// Valor: 3 días hábiles
    /// </summary>
    public const int DiasHabilesProcesoPago = 3;

    /// <summary>
    /// Días máximos para que Almacén confirme una entrega.
    /// Valor: 2 días hábiles
    /// </summary>
    public const int DiasHabilesConfirmacionEntrega = 2;

    /// <summary>
    /// Días máximos para generar el complemento de pago después de la entrega.
    /// Valor: 5 días hábiles
    /// </summary>
    public const int DiasHabilesGeneracionComplemento = 5;

    #endregion

    #region Folio y Ejercicio Fiscal

    /// <summary>
    /// Año mínimo permitido para ejercicios fiscales.
    /// </summary>
    public const int AnioMinimoEjercicioFiscal = 2020;

    /// <summary>
    /// Longitud máxima del número secuencial en el folio DSA.
    /// </summary>
    public const int LongitudMaximaSecuencialFolio = 6;

    #endregion
}

/// <summary>
/// Configuración de cotizaciones requeridas según el monto de la solicitud.
/// Implementa la escalabilidad de requisitos de cotización.
/// </summary>
public static class CotizacionesRequeridas
{
    /// <summary>
    /// Determina el número mínimo de cotizaciones requeridas según el monto.
    /// Por normativa, siempre se requieren mínimo 3 cotizaciones.
    /// </summary>
    /// <param name="monto">Monto de la solicitud en MXN.</param>
    /// <returns>Número mínimo de cotizaciones requeridas.</returns>
    public static int ObtenerCotizacionesRequeridas(decimal monto)
    {
        // Por normativa estatal, siempre se requieren mínimo 3 cotizaciones
        // independientemente del monto para el Fondo Revolvente
        return LimitesNegocio.CotizacionesMinimasRequeridas;
    }

    /// <summary>
    /// Determina si el número de cotizaciones recibidas es suficiente.
    /// </summary>
    /// <param name="monto">Monto de la solicitud.</param>
    /// <param name="cotizacionesRecibidas">Número de cotizaciones recibidas.</param>
    /// <returns>True si las cotizaciones son suficientes.</returns>
    public static bool EsSuficiente(decimal monto, int cotizacionesRecibidas) =>
        cotizacionesRecibidas >= ObtenerCotizacionesRequeridas(monto);

    /// <summary>
    /// Obtiene un mensaje descriptivo sobre el estado de las cotizaciones.
    /// </summary>
    /// <param name="monto">Monto de la solicitud.</param>
    /// <param name="cotizacionesRecibidas">Número de cotizaciones recibidas.</param>
    /// <returns>Mensaje descriptivo del estado.</returns>
    public static string ObtenerMensajeEstado(decimal monto, int cotizacionesRecibidas)
    {
        var requeridas = ObtenerCotizacionesRequeridas(monto);
        var faltantes = requeridas - cotizacionesRecibidas;

        return faltantes switch
        {
            <= 0 => $"Cotizaciones completas ({cotizacionesRecibidas}/{requeridas})",
            1 => $"Falta 1 cotización ({cotizacionesRecibidas}/{requeridas})",
            _ => $"Faltan {faltantes} cotizaciones ({cotizacionesRecibidas}/{requeridas})"
        };
    }
}

/// <summary>
/// Configuración del sistema de bloqueo de edición para prevenir conflictos de concurrencia.
/// Implementación de RN-005.
/// </summary>
public static class ConfiguracionBloqueo
{
    /// <summary>
    /// Duración total del bloqueo de edición.
    /// </summary>
    public static TimeSpan DuracionBloqueo => TimeSpan.FromMinutes(LimitesNegocio.MinutosDuracionBloqueo);

    /// <summary>
    /// Tiempo antes de la expiración para renovar el bloqueo.
    /// </summary>
    public static TimeSpan TiempoRenovacion => TimeSpan.FromMinutes(LimitesNegocio.MinutosRenovacionBloqueo);

    /// <summary>
    /// Determina si un bloqueo está expirado basándose en su fecha de adquisición.
    /// </summary>
    /// <param name="fechaAdquisicion">Fecha y hora cuando se adquirió el bloqueo.</param>
    /// <param name="fechaActual">Fecha y hora actual.</param>
    /// <returns>True si el bloqueo está expirado.</returns>
    public static bool EstaExpirado(DateTime fechaAdquisicion, DateTime fechaActual) =>
        fechaActual - fechaAdquisicion > DuracionBloqueo;

    /// <summary>
    /// Determina si es momento de renovar el bloqueo.
    /// </summary>
    /// <param name="fechaAdquisicion">Fecha y hora cuando se adquirió el bloqueo.</param>
    /// <param name="fechaActual">Fecha y hora actual.</param>
    /// <returns>True si se debe renovar el bloqueo.</returns>
    public static bool DebeRenovar(DateTime fechaAdquisicion, DateTime fechaActual)
    {
        var tiempoTranscurrido = fechaActual - fechaAdquisicion;
        var tiempoRestante = DuracionBloqueo - tiempoTranscurrido;
        return tiempoRestante <= TiempoRenovacion && tiempoRestante > TimeSpan.Zero;
    }

    /// <summary>
    /// Calcula el tiempo restante de un bloqueo.
    /// </summary>
    /// <param name="fechaAdquisicion">Fecha y hora cuando se adquirió el bloqueo.</param>
    /// <param name="fechaActual">Fecha y hora actual.</param>
    /// <returns>Tiempo restante del bloqueo, o TimeSpan.Zero si ya expiró.</returns>
    public static TimeSpan TiempoRestante(DateTime fechaAdquisicion, DateTime fechaActual)
    {
        var tiempoTranscurrido = fechaActual - fechaAdquisicion;
        var tiempoRestante = DuracionBloqueo - tiempoTranscurrido;
        return tiempoRestante > TimeSpan.Zero ? tiempoRestante : TimeSpan.Zero;
    }
}
