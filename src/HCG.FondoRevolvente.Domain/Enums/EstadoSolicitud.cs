using System.ComponentModel;
using System.Runtime.Serialization;

namespace HCG.FondoRevolvente.Domain.Enums;

/// <summary>
/// Define los 30 estados posibles en el ciclo de vida de una solicitud de Fondo Revolvente.
/// <para>
/// Referencia: Sección 18.1 - Los 30 Estados del Ciclo de Vida.
/// Referencia: Sección 1.2 - 38 transiciones de estado gobernadas por SolicitudStateMachine.
/// </para>
/// <remarks>
/// Los estados están organizados en 8 fases secuenciales. Los valores numéricos son explícitos
/// para garantizar integridad con la base de datos y el historial de auditoría.
/// ADVERTENCIA: No modificar los valores numéricos una vez en producción.
/// </remarks>
/// </summary>
public enum EstadoSolicitud
{
    #region Fase 1: Recepción y Validación Inicial (Estados 1-5)

    /// <summary>
    /// Estado inicial. La solicitud ha sido recibida y registrada en el sistema.
    /// Pendiente de revisión documental inicial.
    /// </summary>
    [Description("Recepcionado")]
    [EnumMember(Value = "RECEPCIONADO")]
    Recepcionado = 1,

    /// <summary>
    /// La solicitud está en proceso de revisión documental por parte del Comprador DSA.
    /// Se verifican requisitos normativos y límites del Fondo Revolvente ($75,000 MXN).
    /// </summary>
    [Description("En Revisión")]
    [EnumMember(Value = "EN_REVISION")]
    EnRevision = 2,

    /// <summary>
    /// La documentación ha sido validada correctamente. La solicitud procede a la
    /// siguiente fase (Autorización CAA o Cotización según el monto).
    /// </summary>
    [Description("Validado")]
    [EnumMember(Value = "VALIDADO")]
    Validado = 3,

    /// <summary>
    /// La solicitud fue rechazada durante la validación inicial.
    /// Motivos: documentación incompleta, monto excedido, o incumplimiento normativo.
    /// </summary>
    [Description("Rechazado en Validación")]
    [EnumMember(Value = "RECHAZADO_VALIDACION")]
    RechazadoValidacion = 4,

    /// <summary>
    /// Se detectó posible fraccionamiento de compra (RN-002).
    /// La solicitud está bajo análisis para detectar violaciones al Art. 57 del Reglamento.
    /// </summary>
    [Description("En Análisis de Fraccionamiento")]
    [EnumMember(Value = "EN_FRACCIONAMIENTO")]
    EnFraccionamiento = 5,

    #endregion

    #region Fase 2: Autorización CAA (Estados 6-9)

    /// <summary>
    /// La solicitud ha sido enviada al Comité de Adquisiciones y Arrendamientos (CAA).
    /// Pendiente de revisión y autorización en sesión de comité.
    /// </summary>
    [Description("En Autorización CAA")]
    [EnumMember(Value = "EN_AUTORIZACION_CAA")]
    EnAutorizacionCAA = 6,

    /// <summary>
    /// El CAA ha autorizado la solicitud. Procede a la fase de cotización
    /// o selección de proveedor según corresponda.
    /// </summary>
    [Description("Autorizado por CAA")]
    [EnumMember(Value = "AUTORIZADO_CAA")]
    AutorizadoCAA = 7,

    /// <summary>
    /// El CAA ha rechazado la solicitud.
    /// Se registra el motivo y se notifica al solicitante.
    /// </summary>
    [Description("Rechazado por CAA")]
    [EnumMember(Value = "RECHAZADO_CAA")]
    RechazadoCAA = 8,

    /// <summary>
    /// Solicitud rechazado por CAA pero con posibilidad de corrección.
    /// El Comprador DSA puede subsanar observaciones y reenviar.
    /// </summary>
    [Description("Rechazado CAA - Reintento")]
    [EnumMember(Value = "RECHAZADO_CAA_REINTENTO")]
    RechazadoCAAReintento = 9,

    #endregion

    #region Fase 3: Estudio de Mercado / Cotización (Estados 10-12)

    /// <summary>
    /// La solicitud está lista para cotización pero aún no se han solicitado.
    /// Se requieren mínimo 3 cotizaciones (RN-003).
    /// </summary>
    [Description("Sin Cotizaciones")]
    [EnumMember(Value = "SIN_COTIZACIONES")]
    SinCotizaciones = 10,

    /// <summary>
    /// Se han solicitado cotizaciones a proveedores.
    /// Pendiente de recibir el mínimo requerido (3 cotizaciones).
    /// </summary>
    [Description("En Cotización")]
    [EnumMember(Value = "EN_COTIZACION")]
    EnCotizacion = 11,

    /// <summary>
    /// Se han recibido las cotizaciones necesarias (mínimo 3).
    /// Listo para elaborar cuadro comparativo.
    /// </summary>
    [Description("Cotización Completa")]
    [EnumMember(Value = "COTIZACION_COMPLETA")]
    CotizacionCompleta = 12,

    #endregion

    #region Fase 4: Selección de Proveedor / Pedido (Estados 13-15)

    /// <summary>
    /// Se está elaborando el cuadro comparativo de cotizaciones.
    /// Análisis de precios, especificaciones y condiciones de entrega.
    /// </summary>
    [Description("En Cuadro Comparativo")]
    [EnumMember(Value = "CUADRO_COMPARATIVO")]
    CuadroComparativo = 13,

    /// <summary>
    /// Se ha seleccionado al proveedor ganador basado en el cuadro comparativo.
    /// Pendiente de generar la orden de compra/pedido.
    /// </summary>
    [Description("Proveedor Seleccionado")]
    [EnumMember(Value = "PROVEEDOR_SELECCIONADO")]
    ProveedorSeleccionado = 14,

    /// <summary>
    /// Se ha generado y enviado el pedido al proveedor.
    /// Pendiente de confirmación de recepción y fecha de entrega.
    /// </summary>
    [Description("Pedido Generado")]
    [EnumMember(Value = "PEDIDO_GENERADO")]
    PedidoGenerado = 15,

    #endregion

    #region Fase 5: Entrega de Bienes o Servicios (Estados 16-20)

    /// <summary>
    /// El proveedor ha confirmado el pedido y está en proceso de entrega.
    /// Se espera la fecha de entrega acordada.
    /// </summary>
    [Description("En Entrega")]
    [EnumMember(Value = "EN_ENTREGA")]
    EnEntrega = 16,

    /// <summary>
    /// Los bienes/servicios han sido entregados por el proveedor.
    /// Pendiente de recepción formal en Almacén HCG.
    /// </summary>
    [Description("Entregado por Proveedor")]
    [EnumMember(Value = "ENTREGADO")]
    Entregado = 17,

    /// <summary>
    /// Se detectaron discrepancias entre lo pedido y lo entregado.
    /// Requiere resolución antes de continuar (devolución, reposición, nota de crédito).
    /// </summary>
    [Description("Con Discrepancia")]
    [EnumMember(Value = "CON_DISCREPANCIA")]
    ConDiscrepancia = 18,

    /// <summary>
    /// Los bienes están en proceso de recepción en Almacén HCG.
    /// Verificación de cantidades, especificaciones y estado físico.
    /// </summary>
    [Description("En Recepción de Bienes")]
    [EnumMember(Value = "EN_RECEPCION_BIENES")]
    EnRecepcionBienes = 19,

    /// <summary>
    /// Los bienes han sido recepcionados satisfactoriamente en Almacén.
    /// Acta de recepción firmada y adjuntada al expediente.
    /// </summary>
    [Description("Bienes Recepcionados")]
    [EnumMember(Value = "RECEPCIONADO_BIENES")]
    RecepcionadoBienes = 20,

    #endregion

    #region Fase 6: Validación Fiscal CFDI (Estados 21-24)

    /// <summary>
    /// Se ha solicitado el CFDI al proveedor.
    /// Pendiente de recepción del comprobante fiscal.
    /// </summary>
    [Description("En Validación CFDI")]
    [EnumMember(Value = "EN_VALIDACION_CFDI")]
    EnValidacionCfdi = 21,

    /// <summary>
    /// El CFDI ha sido validado exitosamente ante el SAT (CFDI 4.0).
    /// Coinciden RFC, monto, y concepto con la solicitud.
    /// </summary>
    [Description("CFDI Válido")]
    [EnumMember(Value = "CFDI_VALIDO")]
    CfdiValido = 22,

    /// <summary>
    /// El CFDI no pasó la validación del SAT.
    /// Se requiere corrección por parte del proveedor (RN-004).
    /// </summary>
    [Description("CFDI Inválido")]
    [EnumMember(Value = "CFDI_INVALIDO")]
    CfdiInvalido = 23,

    /// <summary>
    /// El servicio del SAT no está disponible.
    /// Se está reintentando la validación (Polly Circuit Breaker activo).
    /// </summary>
    [Description("Error SAT - Reintentando")]
    [EnumMember(Value = "ERROR_SAT_REINTENTANDO")]
    ErrorSatReintentando = 24,

    #endregion

    #region Fase 7: Pago (Estados 25-27)

    /// <summary>
    /// El pago está en trámite ante Recursos Financieros.
    /// Documentación completa enviada para proceso de pago.
    /// </summary>
    [Description("En Pago")]
    [EnumMember(Value = "EN_PAGO")]
    EnPago = 25,

    /// <summary>
    /// El pago ha sido ejecutado (transferencia/cheque).
    /// Pendiente de confirmación por el proveedor.
    /// </summary>
    [Description("Pagado")]
    [EnumMember(Value = "PAGADO")]
    Pagado = 26,

    /// <summary>
    /// Ocurrió un error durante el proceso de pago.
    /// Requiere intervención de Recursos Financieros.
    /// </summary>
    [Description("Error en Pago")]
    [EnumMember(Value = "ERROR_PAGO")]
    ErrorPago = 27,

    #endregion

    #region Fase 8: Cierre del Expediente (Estados 28-30)

    /// <summary>
    /// El expediente está en revisión final para cierre.
    /// Verificación de documentación completa y cumplimiento normativo.
    /// </summary>
    [Description("En Cierre")]
    [EnumMember(Value = "EN_CIERRE")]
    EnCierre = 28,

    /// <summary>
    /// El expediente ha sido cerrado exitosamente.
    /// Estado final de proceso completado.
    /// </summary>
    [Description("Cerrado")]
    [EnumMember(Value = "CERRADO")]
    Cerrado = 29,

    /// <summary>
    /// El expediente fue cancelado antes de completarse.
    /// Estado final de proceso cancelado.
    /// </summary>
    [Description("Cancelado")]
    [EnumMember(Value = "CANCELADO")]
    Cancelado = 30

    #endregion
}

/// <summary>
/// Métodos de extensión para el enum EstadoSolicitud.
/// Proporciona utilidades para la capa de presentación y lógica de negocio.
/// </summary>
public static class EstadoSolicitudExtensions
{
    /// <summary>
    /// Obtiene la fase del proceso a la que pertenece un estado.
    /// </summary>
    public static FaseProceso ObtenerFase(this EstadoSolicitud estado)
    {
        return estado switch
        {
            >= EstadoSolicitud.Recepcionado and <= EstadoSolicitud.EnFraccionamiento 
                => FaseProceso.RecepcionValidacionInicial,
            >= EstadoSolicitud.EnAutorizacionCAA and <= EstadoSolicitud.RechazadoCAAReintento 
                => FaseProceso.AutorizacionCAA,
            >= EstadoSolicitud.SinCotizaciones and <= EstadoSolicitud.CotizacionCompleta 
                => FaseProceso.EstudioMercadoCotizacion,
            >= EstadoSolicitud.CuadroComparativo and <= EstadoSolicitud.PedidoGenerado 
                => FaseProceso.SeleccionProveedorPedido,
            >= EstadoSolicitud.EnEntrega and <= EstadoSolicitud.RecepcionadoBienes 
                => FaseProceso.EntregaBienesServicios,
            >= EstadoSolicitud.EnValidacionCfdi and <= EstadoSolicitud.ErrorSatReintentando 
                => FaseProceso.ValidacionFiscalCFDI,
            >= EstadoSolicitud.EnPago and <= EstadoSolicitud.ErrorPago 
                => FaseProceso.Pago,
            >= EstadoSolicitud.EnCierre and <= EstadoSolicitud.Cancelado 
                => FaseProceso.CierreExpediente,
            _ => throw new ArgumentOutOfRangeException(nameof(estado))
        };
    }

    /// <summary>
    /// Determina si el estado es un estado final (no permite más transiciones).
    /// </summary>
    public static bool EsEstadoFinal(this EstadoSolicitud estado)
    {
        return estado is EstadoSolicitud.Cerrado or EstadoSolicitud.Cancelado;
    }

    /// <summary>
    /// Determina si el estado representa un rechazo o error.
    /// </summary>
    public static bool EsEstadoNegativo(this EstadoSolicitud estado)
    {
        return estado is 
            EstadoSolicitud.RechazadoValidacion or
            EstadoSolicitud.RechazadoCAA or
            EstadoSolicitud.ConDiscrepancia or
            EstadoSolicitud.CfdiInvalido or
            EstadoSolicitud.ErrorPago or
            EstadoSolicitud.Cancelado;
    }

    /// <summary>
    /// Determina si el estado permite edición del expediente.
    /// Referencia: Sección 3.3 - Gestión Visual del Bloqueo de Edición (RN-005).
    /// </summary>
    public static bool PermiteEdicion(this EstadoSolicitud estado)
    {
        return estado is 
            EstadoSolicitud.Recepcionado or
            EstadoSolicitud.EnRevision or
            EstadoSolicitud.SinCotizaciones or
            EstadoSolicitud.EnCotizacion or
            EstadoSolicitud.RechazadoCAAReintento;
    }

    /// <summary>
    /// Obtiene el color semántico para el StateBadge según Apéndice A.
    /// </summary>
    public static string ObtenerColorSemantic(this EstadoSolicitud estado)
    {
        return estado switch
        {
            EstadoSolicitud.Validado => "Success",
            EstadoSolicitud.AutorizadoCAA => "Success",
            EstadoSolicitud.CotizacionCompleta => "Success",
            EstadoSolicitud.ProveedorSeleccionado => "Success",
            EstadoSolicitud.RecepcionadoBienes => "Success",
            EstadoSolicitud.CfdiValido => "Success",
            EstadoSolicitud.Pagado => "Success",
            EstadoSolicitud.Cerrado => "Success",

            EstadoSolicitud.RechazadoValidacion => "Critical",
            EstadoSolicitud.RechazadoCAA => "Critical",
            EstadoSolicitud.CfdiInvalido => "Critical",
            EstadoSolicitud.ErrorPago => "Critical",
            EstadoSolicitud.Cancelado => "Critical",

            EstadoSolicitud.EnFraccionamiento => "Caution",
            EstadoSolicitud.RechazadoCAAReintento => "Caution",
            EstadoSolicitud.ConDiscrepancia => "Caution",
            EstadoSolicitud.ErrorSatReintentando => "Caution",

            EstadoSolicitud.EnAutorizacionCAA => "Attention",
            EstadoSolicitud.EnCotizacion => "Attention",
            EstadoSolicitud.CuadroComparativo => "Attention",
            EstadoSolicitud.EnEntrega => "Attention",
            EstadoSolicitud.EnValidacionCfdi => "Attention",
            EstadoSolicitud.EnPago => "Attention",
            EstadoSolicitud.EnCierre => "Attention",

            _ => "Neutral"
        };
    }

    /// <summary>
    /// Obtiene el ícono Segoe Fluent Icons para el estado.
    /// </summary>
    public static string ObtenerIcono(this EstadoSolicitud estado)
    {
        return estado switch
        {
            EstadoSolicitud.Recepcionado => "\uE8F1",
            EstadoSolicitud.EnRevision => "\uE79D",
            EstadoSolicitud.Validado => "\uE73E",
            EstadoSolicitud.RechazadoValidacion => "\uE711",
            EstadoSolicitud.EnFraccionamiento => "\uE7BA",
            EstadoSolicitud.EnAutorizacionCAA => "\uE724",
            EstadoSolicitud.AutorizadoCAA => "\uE73E",
            EstadoSolicitud.RechazadoCAA => "\uE711",
            EstadoSolicitud.RechazadoCAAReintento => "\uE725",
            EstadoSolicitud.SinCotizaciones => "\uE738",
            EstadoSolicitud.EnCotizacion => "\uE723",
            EstadoSolicitud.CotizacionCompleta => "\uE730",
            EstadoSolicitud.CuadroComparativo => "\uE8FD",
            EstadoSolicitud.ProveedorSeleccionado => "\uE734",
            EstadoSolicitud.PedidoGenerado => "\uE8F0",
            EstadoSolicitud.EnEntrega => "\uE806",
            EstadoSolicitud.Entregado => "\uE79D",
            EstadoSolicitud.ConDiscrepancia => "\uE7BA",
            EstadoSolicitud.EnRecepcionBienes => "\uE806",
            EstadoSolicitud.RecepcionadoBienes => "\uE73E",
            EstadoSolicitud.EnValidacionCfdi => "\uE8F0",
            EstadoSolicitud.CfdiValido => "\uE73E",
            EstadoSolicitud.CfdiInvalido => "\uE711",
            EstadoSolicitud.ErrorSatReintentando => "\uE712",
            EstadoSolicitud.EnPago => "\uE775",
            EstadoSolicitud.Pagado => "\uE73E",
            EstadoSolicitud.ErrorPago => "\uE711",
            EstadoSolicitud.EnCierre => "\uE74E",
            EstadoSolicitud.Cerrado => "\uE73E",
            EstadoSolicitud.Cancelado => "\uE711",
            _ => "\uE783"
        };
    }
}
