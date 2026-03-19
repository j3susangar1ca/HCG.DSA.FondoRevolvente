using System.ComponentModel;
using System.Runtime.Serialization;

namespace HCG.FondoRevolvente.Domain.Enums;

/// <summary>
/// Define los 27 tipos de hitos que marcan el progreso de una solicitud
/// a través de las 8 fases del proceso de Fondo Revolvente.
/// <para>
/// Referencia: Sección 11 - Módulo 07 (Timeline de Hitos: Las 8 Fases del Proceso).
/// Referencia: Sección 1.2 - 27 hitos de proceso, distribuidos en 8 fases secuenciales.
/// </para>
/// <remarks>
/// Los hitos son eventos significativos que quedan registrados en el timeline
/// de cada solicitud. Cada hito tiene asociado: fecha, usuario responsable,
/// documentos adjuntos (opcional) y observaciones.
/// ADVERTENCIA: No modificar los valores numéricos una vez en producción
/// para mantener la compatibilidad con el historial de auditoría.
/// </remarks>
/// </summary>
public enum TipoHito
{
    #region Fase 1: Recepción y Validación Inicial (3 hitos)

    /// <summary>
    /// Recepción del oficio de solicitud de compra.
    /// Documento inicial que da inicio al expediente.
    /// Responsable: Comprador DSA.
    /// </summary>
    [Description("Recepción de Oficio")]
    [EnumMember(Value = "RECEPCION_OFICIO")]
    RecepcionOficio = 1,

    /// <summary>
    /// Asignación del folio DSA a la solicitud.
    /// Formato: FR-AAAA-XXXXX (Fondo Revolvente-Año-Consecutivo).
    /// Responsable: Sistema (automático) / Comprador DSA.
    /// </summary>
    [Description("Asignación de Folio")]
    [EnumMember(Value = "ASIGNACION_FOLIO")]
    AsignacionFolio = 2,

    /// <summary>
    /// Validación documental inicial completada.
    /// Se verifican requisitos normativos y límite de $75,000 MXN (RN-001).
    /// Responsable: Comprador DSA.
    /// </summary>
    [Description("Validación Documental Inicial")]
    [EnumMember(Value = "VALIDACION_DOCUMENTAL_INICIAL")]
    ValidacionDocumentalInicial = 3,

    #endregion

    #region Fase 2: Autorización CAA (4 hitos)

    /// <summary>
    /// Envío del expediente al Comité de Adquisiciones y Arrendamientos.
    /// Se programa para revisión en sesión de comité.
    /// Responsable: Comprador DSA.
    /// </summary>
    [Description("Envío a Comité CAA")]
    [EnumMember(Value = "ENVIO_COMITE_CAA")]
    EnvioComiteCAA = 4,

    /// <summary>
    /// Revisión del expediente por el CAA.
    /// Evaluación de la solicitud en sesión de comité.
    /// Responsable: Miembro CAA.
    /// </summary>
    [Description("Revisión CAA")]
    [EnumMember(Value = "REVISION_CAA")]
    RevisionCAA = 5,

    /// <summary>
    /// Resolución del CAA (autorización o rechazo).
    /// Se registra el acuerdo del comité.
    /// Responsable: Miembro CAA.
    /// </summary>
    [Description("Resolución CAA")]
    [EnumMember(Value = "RESOLUCION_CAA")]
    ResolucionCAA = 6,

    /// <summary>
    /// Reenvío del expediente con correcciones tras rechazo CAA.
    /// Se subsanan observaciones del comité.
    /// Responsable: Comprador DSA.
    /// </summary>
    [Description("Reenvío con Correcciones")]
    [EnumMember(Value = "REENVIO_CORRECCION")]
    ReenvioCorreccion = 7,

    #endregion

    #region Fase 3: Estudio de Mercado / Cotización (4 hitos)

    /// <summary>
    /// Solicitud de cotizaciones a proveedores.
    /// Se envían solicitudes a mínimo 3 proveedores (RN-003).
    /// Responsable: Comprador DSA.
    /// </summary>
    [Description("Solicitud de Cotizaciones")]
    [EnumMember(Value = "SOLICITUD_COTIZACIONES")]
    SolicitudCotizaciones = 8,

    /// <summary>
    /// Recepción de cotización de proveedor.
    /// Se registra cada cotización individualmente (mínimo 3).
    /// Responsable: Comprador DSA.
    /// NOTA: Este hito es repetible (puede registrarse múltiples veces).
    /// </summary>
    [Description("Recepción de Cotización")]
    [EnumMember(Value = "RECEPCION_COTIZACION_PROVEEDOR")]
    RecepcionCotizacionProveedor = 9,

    /// <summary>
    /// Cotizaciones completas (mínimo 3 recibidas).
    /// Se cumple el requisito de cotizaciones múltiples.
    /// Responsable: Sistema (automático) / Comprador DSA.
    /// </summary>
    [Description("Cotizaciones Completas")]
    [EnumMember(Value = "COTIZACIONES_COMPLETAS")]
    CotizacionesCompletas = 10,

    /// <summary>
    /// Validación de cotizaciones recibidas.
    /// Verificación de documentos y precios.
    /// Responsable: Comprador DSA.
    /// </summary>
    [Description("Validación de Cotizaciones")]
    [EnumMember(Value = "VALIDACION_COTIZACIONES")]
    ValidacionCotizaciones = 11,

    #endregion

    #region Fase 4: Selección de Proveedor / Pedido (3 hitos)

    /// <summary>
    /// Elaboración del cuadro comparativo de cotizaciones.
    /// Análisis de precios, especificaciones y condiciones.
    /// Documento PDF generado para el expediente.
    /// Responsable: Comprador DSA.
    /// </summary>
    [Description("Elaboración de Cuadro Comparativo")]
    [EnumMember(Value = "ELABORACION_CUADRO_COMPARATIVO")]
    ElaboracionCuadroComparativo = 12,

    /// <summary>
    /// Selección del proveedor ganador.
    /// Basado en el análisis del cuadro comparativo.
    /// Responsable: Comprador DSA.
    /// </summary>
    [Description("Selección de Proveedor")]
    [EnumMember(Value = "SELECCION_PROVEEDOR")]
    SeleccionProveedor = 13,

    /// <summary>
    /// Generación de orden de compra/pedido.
    /// Documento PDF enviado al proveedor.
    /// Responsable: Comprador DSA.
    /// </summary>
    [Description("Generación de Orden de Compra")]
    [EnumMember(Value = "GENERACION_ORDEN_COMPRA")]
    GeneracionOrdenCompra = 14,

    #endregion

    #region Fase 5: Entrega de Bienes o Servicios (3 hitos)

    /// <summary>
    /// Confirmación de pedido por el proveedor.
    /// El proveedor acepta y confirma fecha de entrega.
    /// Responsable: Proveedor / Comprador DSA.
    /// </summary>
    [Description("Confirmación de Pedido")]
    [EnumMember(Value = "CONFIRMACION_PEDIDO_PROVEEDOR")]
    ConfirmacionPedidoProveedor = 15,

    /// <summary>
    /// Notificación de entrega por el proveedor.
    /// El proveedor informa que los bienes están listos para entrega.
    /// Responsable: Proveedor / Comprador DSA.
    /// </summary>
    [Description("Notificación de Entrega")]
    [EnumMember(Value = "NOTIFICACION_ENTREGA")]
    NotificacionEntrega = 16,

    /// <summary>
    /// Recepción de bienes en Almacén HCG.
    /// Acta de recepción firmada y adjuntada al expediente.
    /// Responsable: Almacén.
    /// </summary>
    [Description("Recepción en Almacén HCG")]
    [EnumMember(Value = "RECEPCION_ALMACEN_HCG")]
    RecepcionAlmacenHCG = 17,

    #endregion

    #region Fase 6: Validación Fiscal CFDI (4 hitos)

    /// <summary>
    /// Solicitud de CFDI al proveedor.
    /// Se requiere factura CFDI 4.0 para continuar.
    /// Responsable: Comprador DSA.
    /// </summary>
    [Description("Solicitud de CFDI")]
    [EnumMember(Value = "SOLICITUD_CFDI_PROVEEDOR")]
    SolicitudCFDIProveedor = 18,

    /// <summary>
    /// Carga del archivo XML del CFDI al sistema.
    /// Archivos XML y PDF adjuntos al expediente.
    /// Responsable: Comprador DSA / Finanzas.
    /// </summary>
    [Description("Carga de XML al Sistema")]
    [EnumMember(Value = "CARGA_XML_SISTEMA")]
    CargaXMLSistema = 19,

    /// <summary>
    /// Validación del CFDI ante el SAT.
    /// Proceso automático vía Web Service SAT (CFDI 4.0).
    /// Referencia: Sección 14 - Módulo 10 (Validación Fiscal).
    /// Responsable: Sistema (automático) / Finanzas.
    /// </summary>
    [Description("Validación ante SAT")]
    [EnumMember(Value = "VALIDACION_ANTE_SAT")]
    ValidacionAnteSAT = 20,

    /// <summary>
    /// Confirmación de CFDI válido.
    /// El comprobante fiscal pasó la validación del SAT.
    /// Responsable: Finanzas.
    /// </summary>
    [Description("Confirmación de CFDI Válido")]
    [EnumMember(Value = "CONFIRMACION_CFDI_VALIDO")]
    ConfirmacionCFDIValido = 21,

    #endregion

    #region Fase 7: Pago (3 hitos)

    /// <summary>
    /// Trámite de pago ante Recursos Financieros.
    /// Requisición de pago generada en sistema financiero.
    /// Responsable: Finanzas.
    /// </summary>
    [Description("Trámite de Pago")]
    [EnumMember(Value = "TRAMITE_PAGO_RECURSOS_FINANCIEROS")]
    TramitePagoRecursosFinancieros = 22,

    /// <summary>
    /// Registro de transferencia bancaria o cheque.
    /// Comprobante de pago adjunto al expediente.
    /// Responsable: Finanzas.
    /// </summary>
    [Description("Registro de Pago")]
    [EnumMember(Value = "REGISTRO_TRANSFERENCIA_CHEQUE")]
    RegistroTransferenciaCheque = 23,

    /// <summary>
    /// Confirmación de pago ejecutado.
    /// El proveedor confirma recepción del pago.
    /// Responsable: Finanzas / Proveedor.
    /// </summary>
    [Description("Confirmación de Pago Ejecutado")]
    [EnumMember(Value = "CONFIRMACION_PAGO_EJECUTADO")]
    ConfirmacionPagoEjecutado = 24,

    #endregion

    #region Fase 8: Cierre del Expediente (3 hitos)

    /// <summary>
    /// Carga del complemento de pago CFDI.
    /// Documento fiscal requerido para cierre.
    /// Responsable: Finanzas.
    /// </summary>
    [Description("Carga de Complemento de Pago")]
    [EnumMember(Value = "CARGA_COMPLEMENTO_PAGO")]
    CargaComplementoPago = 25,

    /// <summary>
    /// Revisión de expediente completo.
    /// Verificación de documentación completa y cumplimiento normativo.
    /// Responsable: Comprador DSA.
    /// </summary>
    [Description("Revisión de Expediente Completo")]
    [EnumMember(Value = "REVISION_EXPEDIENTE_COMPLETO")]
    RevisionExpedienteCompleto = 26,

    /// <summary>
    /// Cierre oficial del expediente.
    /// El proceso de Fondo Revolvente ha concluido exitosamente.
    /// Responsable: Comprador DSA / Administrador.
    /// </summary>
    [Description("Cierre Oficial de Expediente")]
    [EnumMember(Value = "CIERRE_OFICIAL_EXPEDIENTE")]
    CierreOficialExpediente = 27

    #endregion
}

/// <summary>
/// Métodos de extensión para el enum TipoHito.
/// Proporciona utilidades para la capa de presentación, lógica de negocio e integración UI.
/// </summary>
public static class TipoHitoExtensions
{
    #region Información de Fase

    /// <summary>
    /// Obtiene la fase del proceso a la que pertenece un hito.
    /// </summary>
    public static FaseProceso ObtenerFase(this TipoHito hito)
    {
        return hito switch
        {
            >= TipoHito.RecepcionOficio and <= TipoHito.ValidacionDocumentalInicial 
                => FaseProceso.RecepcionValidacionInicial,
            >= TipoHito.EnvioComiteCAA and <= TipoHito.ReenvioCorreccion 
                => FaseProceso.AutorizacionCAA,
            >= TipoHito.SolicitudCotizaciones and <= TipoHito.ValidacionCotizaciones 
                => FaseProceso.EstudioMercadoCotizacion,
            >= TipoHito.ElaboracionCuadroComparativo and <= TipoHito.GeneracionOrdenCompra 
                => FaseProceso.SeleccionProveedorPedido,
            >= TipoHito.ConfirmacionPedidoProveedor and <= TipoHito.RecepcionAlmacenHCG 
                => FaseProceso.EntregaBienesServicios,
            >= TipoHito.SolicitudCFDIProveedor and <= TipoHito.ConfirmacionCFDIValido 
                => FaseProceso.ValidacionFiscalCFDI,
            >= TipoHito.TramitePagoRecursosFinancieros and <= TipoHito.ConfirmacionPagoEjecutado 
                => FaseProceso.Pago,
            >= TipoHito.CargaComplementoPago and <= TipoHito.CierreOficialExpediente 
                => FaseProceso.CierreExpediente,
            _ => throw new ArgumentOutOfRangeException(nameof(hito))
        };
    }

    /// <summary>
    /// Obtiene el orden del hito dentro de su fase (1-based).
    /// </summary>
    public static int ObtenerOrdenEnFase(this TipoHito hito)
    {
        return hito switch
        {
            // Fase 1
            TipoHito.RecepcionOficio => 1,
            TipoHito.AsignacionFolio => 2,
            TipoHito.ValidacionDocumentalInicial => 3,
            // Fase 2
            TipoHito.EnvioComiteCAA => 1,
            TipoHito.RevisionCAA => 2,
            TipoHito.ResolucionCAA => 3,
            TipoHito.ReenvioCorreccion => 4,
            // Fase 3
            TipoHito.SolicitudCotizaciones => 1,
            TipoHito.RecepcionCotizacionProveedor => 2,
            TipoHito.CotizacionesCompletas => 3,
            TipoHito.ValidacionCotizaciones => 4,
            // Fase 4
            TipoHito.ElaboracionCuadroComparativo => 1,
            TipoHito.SeleccionProveedor => 2,
            TipoHito.GeneracionOrdenCompra => 3,
            // Fase 5
            TipoHito.ConfirmacionPedidoProveedor => 1,
            TipoHito.NotificacionEntrega => 2,
            TipoHito.RecepcionAlmacenHCG => 3,
            // Fase 6
            TipoHito.SolicitudCFDIProveedor => 1,
            TipoHito.CargaXMLSistema => 2,
            TipoHito.ValidacionAnteSAT => 3,
            TipoHito.ConfirmacionCFDIValido => 4,
            // Fase 7
            TipoHito.TramitePagoRecursosFinancieros => 1,
            TipoHito.RegistroTransferenciaCheque => 2,
            TipoHito.ConfirmacionPagoEjecutado => 3,
            // Fase 8
            TipoHito.CargaComplementoPago => 1,
            TipoHito.RevisionExpedienteCompleto => 2,
            TipoHito.CierreOficialExpediente => 3,
            _ => 0
        };
    }

    /// <summary>
    /// Obtiene todos los hitos de una fase específica.
    /// </summary>
    public static IEnumerable<TipoHito> ObtenerHitosPorFase(FaseProceso fase)
    {
        return fase switch
        {
            FaseProceso.RecepcionValidacionInicial => new[]
            {
                TipoHito.RecepcionOficio,
                TipoHito.AsignacionFolio,
                TipoHito.ValidacionDocumentalInicial
            },
            FaseProceso.AutorizacionCAA => new[]
            {
                TipoHito.EnvioComiteCAA,
                TipoHito.RevisionCAA,
                TipoHito.ResolucionCAA,
                TipoHito.ReenvioCorreccion
            },
            FaseProceso.EstudioMercadoCotizacion => new[]
            {
                TipoHito.SolicitudCotizaciones,
                TipoHito.RecepcionCotizacionProveedor,
                TipoHito.CotizacionesCompletas,
                TipoHito.ValidacionCotizaciones
            },
            FaseProceso.SeleccionProveedorPedido => new[]
            {
                TipoHito.ElaboracionCuadroComparativo,
                TipoHito.SeleccionProveedor,
                TipoHito.GeneracionOrdenCompra
            },
            FaseProceso.EntregaBienesServicios => new[]
            {
                TipoHito.ConfirmacionPedidoProveedor,
                TipoHito.NotificacionEntrega,
                TipoHito.RecepcionAlmacenHCG
            },
            FaseProceso.ValidacionFiscalCFDI => new[]
            {
                TipoHito.SolicitudCFDIProveedor,
                TipoHito.CargaXMLSistema,
                TipoHito.ValidacionAnteSAT,
                TipoHito.ConfirmacionCFDIValido
            },
            FaseProceso.Pago => new[]
            {
                TipoHito.TramitePagoRecursosFinancieros,
                TipoHito.RegistroTransferenciaCheque,
                TipoHito.ConfirmacionPagoEjecutado
            },
            FaseProceso.CierreExpediente => new[]
            {
                TipoHito.CargaComplementoPago,
                TipoHito.RevisionExpedienteCompleto,
                TipoHito.CierreOficialExpediente
            },
            _ => Array.Empty<TipoHito>()
        };
    }

    #endregion

    #region Documentos

    /// <summary>
    /// Determina si el hito requiere documento adjunto obligatorio.
    /// </summary>
    public static bool RequiereDocumento(this TipoHito hito)
    {
        return hito switch
        {
            TipoHito.RecepcionOficio => true,
            TipoHito.ResolucionCAA => true,
            TipoHito.ElaboracionCuadroComparativo => true,
            TipoHito.GeneracionOrdenCompra => true,
            TipoHito.RecepcionAlmacenHCG => true,
            TipoHito.CargaXMLSistema => true,
            TipoHito.RegistroTransferenciaCheque => true,
            TipoHito.CargaComplementoPago => true,
            _ => false
        };
    }

    /// <summary>
    /// Obtiene el tipo de documento esperado para el hito.
    /// </summary>
    public static string? ObtenerTipoDocumentoEsperado(this TipoHito hito)
    {
        return hito switch
        {
            TipoHito.RecepcionOficio => "Oficio de Solicitud (PDF)",
            TipoHito.ResolucionCAA => "Acuerdo CAA (PDF)",
            TipoHito.ElaboracionCuadroComparativo => "Cuadro Comparativo (PDF)",
            TipoHito.GeneracionOrdenCompra => "Orden de Compra (PDF)",
            TipoHito.RecepcionAlmacenHCG => "Acta de Recepción (PDF)",
            TipoHito.CargaXMLSistema => "CFDI (XML + PDF)",
            TipoHito.RegistroTransferenciaCheque => "Comprobante de Pago (PDF)",
            TipoHito.CargaComplementoPago => "Complemento de Pago (XML + PDF)",
            _ => null
        };
    }

    #endregion

    #region Eventos Especiales

    /// <summary>
    /// Determina si el hito representa un evento negativo.
    /// </summary>
    public static bool EsEventoNegativo(this TipoHito hito)
    {
        return hito == TipoHito.ReenvioCorreccion;
    }

    /// <summary>
    /// Determina si el hito es el hito final del proceso.
    /// </summary>
    public static bool EsHitoFinal(this TipoHito hito)
    {
        return hito == TipoHito.CierreOficialExpediente;
    }

    /// <summary>
    /// Determina si el hito puede registrarse múltiples veces.
    /// </summary>
    public static bool EsRepetible(this TipoHito hito)
    {
        return hito == TipoHito.RecepcionCotizacionProveedor;
    }

    #endregion

    #region UI/UX

    /// <summary>
    /// Obtiene el ícono Segoe Fluent Icons asociado al hito.
    /// </summary>
    public static string ObtenerIcono(this TipoHito hito)
    {
        return hito switch
        {
            TipoHito.RecepcionOficio => "\uE8F1",
            TipoHito.AsignacionFolio => "\uE736",
            TipoHito.ValidacionDocumentalInicial => "\uE73E",
            TipoHito.EnvioComiteCAA => "\uE724",
            TipoHito.RevisionCAA => "\uE79D",
            TipoHito.ResolucionCAA => "\uE73E",
            TipoHito.ReenvioCorreccion => "\uE725",
            TipoHito.SolicitudCotizaciones => "\uE738",
            TipoHito.RecepcionCotizacionProveedor => "\uE723",
            TipoHito.CotizacionesCompletas => "\uE730",
            TipoHito.ValidacionCotizaciones => "\uE73E",
            TipoHito.ElaboracionCuadroComparativo => "\uE8FD",
            TipoHito.SeleccionProveedor => "\uE734",
            TipoHito.GeneracionOrdenCompra => "\uE8F0",
            TipoHito.ConfirmacionPedidoProveedor => "\uE73E",
            TipoHito.NotificacionEntrega => "\uE806",
            TipoHito.RecepcionAlmacenHCG => "\uE73E",
            TipoHito.SolicitudCFDIProveedor => "\uE738",
            TipoHito.CargaXMLSistema => "\uE8F0",
            TipoHito.ValidacionAnteSAT => "\uE79D",
            TipoHito.ConfirmacionCFDIValido => "\uE73E",
            TipoHito.TramitePagoRecursosFinancieros => "\uE775",
            TipoHito.RegistroTransferenciaCheque => "\uE775",
            TipoHito.ConfirmacionPagoEjecutado => "\uE73E",
            TipoHito.CargaComplementoPago => "\uE8F0",
            TipoHito.RevisionExpedienteCompleto => "\uE79D",
            TipoHito.CierreOficialExpediente => "\uE74E",
            _ => "\uE783"
        };
    }

    /// <summary>
    /// Obtiene el color semántico para el hito en el Timeline.
    /// </summary>
    public static string ObtenerColorSemantic(this TipoHito hito)
    {
        if (hito.EsEventoNegativo()) return "Caution";
        if (hito.EsHitoFinal()) return "Success";
        return "Attention";
    }

    #endregion

    #region Roles y Responsables

    /// <summary>
    /// Obtiene el rol responsable de registrar el hito.
    /// </summary>
    public static RolAplicacion ObtenerRolResponsable(this TipoHito hito)
    {
        return hito switch
        {
            TipoHito.RecepcionOficio => RolAplicacion.CompradorDSA,
            TipoHito.AsignacionFolio => RolAplicacion.CompradorDSA,
            TipoHito.ValidacionDocumentalInicial => RolAplicacion.CompradorDSA,
            TipoHito.EnvioComiteCAA => RolAplicacion.CompradorDSA,
            TipoHito.RevisionCAA => RolAplicacion.RevisorCAA,
            TipoHito.ResolucionCAA => RolAplicacion.RevisorCAA,
            TipoHito.ReenvioCorreccion => RolAplicacion.CompradorDSA,
            TipoHito.SolicitudCotizaciones => RolAplicacion.CompradorDSA,
            TipoHito.RecepcionCotizacionProveedor => RolAplicacion.CompradorDSA,
            TipoHito.CotizacionesCompletas => RolAplicacion.CompradorDSA,
            TipoHito.ValidacionCotizaciones => RolAplicacion.CompradorDSA,
            TipoHito.ElaboracionCuadroComparativo => RolAplicacion.CompradorDSA,
            TipoHito.SeleccionProveedor => RolAplicacion.CompradorDSA,
            TipoHito.GeneracionOrdenCompra => RolAplicacion.CompradorDSA,
            TipoHito.ConfirmacionPedidoProveedor => RolAplicacion.CompradorDSA,
            TipoHito.NotificacionEntrega => RolAplicacion.CompradorDSA,
            TipoHito.RecepcionAlmacenHCG => RolAplicacion.Almacen,
            TipoHito.SolicitudCFDIProveedor => RolAplicacion.CompradorDSA,
            TipoHito.CargaXMLSistema => RolAplicacion.Finanzas,
            TipoHito.ValidacionAnteSAT => RolAplicacion.Finanzas,
            TipoHito.ConfirmacionCFDIValido => RolAplicacion.Finanzas,
            TipoHito.TramitePagoRecursosFinancieros => RolAplicacion.Finanzas,
            TipoHito.RegistroTransferenciaCheque => RolAplicacion.Finanzas,
            TipoHito.ConfirmacionPagoEjecutado => RolAplicacion.Finanzas,
            TipoHito.CargaComplementoPago => RolAplicacion.Finanzas,
            TipoHito.RevisionExpedienteCompleto => RolAplicacion.CompradorDSA,
            TipoHito.CierreOficialExpediente => RolAplicacion.CompradorDSA,
            _ => RolAplicacion.ConsultaDSA
        };
    }

    /// <summary>
    /// Determina si un rol específico puede registrar este hito.
    /// </summary>
    public static bool RolPuedeRegistrar(this TipoHito hito, RolAplicacion rol)
    {
        if (rol == RolAplicacion.Administrador) return true;
        return hito.ObtenerRolResponsable() == rol;
    }

    #endregion
}
