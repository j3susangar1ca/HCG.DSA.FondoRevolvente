using System.ComponentModel;
using System.Runtime.Serialization;

namespace HCG.FondoRevolvente.Domain.Enums;

/// <summary>
/// Define las 8 fases secuenciales del proceso de adquisición mediante Fondo Revolvente.
/// <para>
/// Referencia: Sección 11 - Módulo 07 (Timeline de Hitos: Las 8 Fases del Proceso).
/// Referencia: Apéndice A - Sistema de Colores por Fase (StateBadge).
/// </para>
/// <remarks>
/// Cada fase agrupa múltiples estados del enum <see cref="EstadoSolicitud"/> y representa
/// una etapa distintiva del ciclo de vida. Las fases son secuenciales y deben completarse
/// antes de avanzar a la siguiente, excepto en casos de rechazo o cancelación.
/// </remarks>
/// </summary>
public enum FaseProceso
{
    /// <summary>
    /// Fase 1: Recepción y Validación Inicial.
    /// <para>Estados: Recepcionado, EnRevision, Validado, RechazadoValidacion, EnFraccionamiento.</para>
    /// <para>Responsable: Comprador DSA.</para>
    /// <para>Color UI: Azul Windows (#0078D4).</para>
    /// </summary>
    [Description("Recepción y Validación Inicial")]
    [EnumMember(Value = "FASE_1_RECEPCION_VALIDACION")]
    RecepcionValidacionInicial = 1,

    /// <summary>
    /// Fase 2: Autorización CAA.
    /// <para>Estados: EnAutorizacionCAA, AutorizadoCAA, RechazadoCAA, RechazadoCAAReintento.</para>
    /// <para>Responsable: Miembro CAA.</para>
    /// <para>Color UI: Amarillo Ámbar (#FFB900).</para>
    /// </summary>
    [Description("Autorización CAA")]
    [EnumMember(Value = "FASE_2_AUTORIZACION_CAA")]
    AutorizacionCAA = 2,

    /// <summary>
    /// Fase 3: Estudio de Mercado / Cotización.
    /// <para>Estados: SinCotizaciones, EnCotizacion, CotizacionCompleta.</para>
    /// <para>Responsable: Comprador DSA.</para>
    /// <para>Color UI: Cyan (#00B7C3).</para>
    /// </summary>
    [Description("Estudio de Mercado / Cotización")]
    [EnumMember(Value = "FASE_3_ESTUDIO_MERCADO")]
    EstudioMercadoCotizacion = 3,

    /// <summary>
    /// Fase 4: Selección de Proveedor / Pedido.
    /// <para>Estados: CuadroComparativo, ProveedorSeleccionado, PedidoGenerado.</para>
    /// <para>Responsable: Comprador DSA.</para>
    /// <para>Color UI: Naranja Cálido (#FF8C00).</para>
    /// </summary>
    [Description("Selección de Proveedor / Pedido")]
    [EnumMember(Value = "FASE_4_SELECCION_PROVEEDOR")]
    SeleccionProveedorPedido = 4,

    /// <summary>
    /// Fase 5: Entrega de Bienes o Servicios.
    /// <para>Estados: EnEntrega, Entregado, ConDiscrepancia, EnRecepcionBienes, RecepcionadoBienes.</para>
    /// <para>Responsable: Almacén.</para>
    /// <para>Color UI: Verde Claro (#6CCB5F).</para>
    /// </summary>
    [Description("Entrega de Bienes o Servicios")]
    [EnumMember(Value = "FASE_5_ENTREGA_BIENES")]
    EntregaBienesServicios = 5,

    /// <summary>
    /// Fase 6: Validación Fiscal CFDI.
    /// <para>Estados: EnValidacionCfdi, CfdiValido, CfdiInvalido, ErrorSatReintentando.</para>
    /// <para>Responsable: Recursos Financieros.</para>
    /// <para>Integración: SAT Web Service (CFDI 4.0).</para>
    /// <para>Color UI: Púrpura Medio (#8764B8).</para>
    /// </summary>
    [Description("Validación Fiscal CFDI")]
    [EnumMember(Value = "FASE_6_VALIDACION_FISCAL")]
    ValidacionFiscalCFDI = 6,

    /// <summary>
    /// Fase 7: Pago.
    /// <para>Estados: EnPago, Pagado, ErrorPago.</para>
    /// <para>Responsable: Recursos Financieros.</para>
    /// <para>Color UI: Verde Oscuro (#107C10).</para>
    /// </summary>
    [Description("Pago")]
    [EnumMember(Value = "FASE_7_PAGO")]
    Pago = 7,

    /// <summary>
    /// Fase 8: Cierre del Expediente.
    /// <para>Estados: EnCierre, Cerrado, Cancelado.</para>
    /// <para>Responsable: Comprador DSA / Administrador.</para>
    /// <para>Color UI: Gris Azulado (#69797E).</para>
    /// </summary>
    [Description("Cierre del Expediente")]
    [EnumMember(Value = "FASE_8_CIERRE")]
    CierreExpediente = 8
}

/// <summary>
/// Métodos de extensión para el enum FaseProceso.
/// Proporciona utilidades para la capa de presentación, lógica de negocio e integración UI.
/// </summary>
public static class FaseProcesoExtensions
{
    #region Información de Fase

    /// <summary>
    /// Obtiene el número de fase (1-8) para una fase dada.
    /// </summary>
    public static int ObtenerNumeroFase(this FaseProceso fase)
    {
        return (int)fase;
    }

    /// <summary>
    /// Obtiene la fase siguiente en el proceso secuencial normal.
    /// Retorna null si la fase actual es la última (Cierre).
    /// </summary>
    public static FaseProceso? ObtenerFaseSiguiente(this FaseProceso fase)
    {
        return fase switch
        {
            FaseProceso.RecepcionValidacionInicial => FaseProceso.AutorizacionCAA,
            FaseProceso.AutorizacionCAA => FaseProceso.EstudioMercadoCotizacion,
            FaseProceso.EstudioMercadoCotizacion => FaseProceso.SeleccionProveedorPedido,
            FaseProceso.SeleccionProveedorPedido => FaseProceso.EntregaBienesServicios,
            FaseProceso.EntregaBienesServicios => FaseProceso.ValidacionFiscalCFDI,
            FaseProceso.ValidacionFiscalCFDI => FaseProceso.Pago,
            FaseProceso.Pago => FaseProceso.CierreExpediente,
            FaseProceso.CierreExpediente => null,
            _ => null
        };
    }

    /// <summary>
    /// Obtiene la fase anterior en el proceso secuencial.
    /// Retorna null si la fase actual es la primera (Recepción).
    /// </summary>
    public static FaseProceso? ObtenerFaseAnterior(this FaseProceso fase)
    {
        return fase switch
        {
            FaseProceso.RecepcionValidacionInicial => null,
            FaseProceso.AutorizacionCAA => FaseProceso.RecepcionValidacionInicial,
            FaseProceso.EstudioMercadoCotizacion => FaseProceso.AutorizacionCAA,
            FaseProceso.SeleccionProveedorPedido => FaseProceso.EstudioMercadoCotizacion,
            FaseProceso.EntregaBienesServicios => FaseProceso.SeleccionProveedorPedido,
            FaseProceso.ValidacionFiscalCFDI => FaseProceso.EntregaBienesServicios,
            FaseProceso.Pago => FaseProceso.ValidacionFiscalCFDI,
            FaseProceso.CierreExpediente => FaseProceso.Pago,
            _ => null
        };
    }

    #endregion

    #region Permisos y Edición

    /// <summary>
    /// Determina si la fase permite edición del expediente por el Comprador DSA.
    /// Solo las fases iniciales permiten edición completa.
    /// Referencia: Sección 3.3 - Gestión Visual del Bloqueo de Edición (RN-005).
    /// </summary>
    public static bool PermiteEdicion(this FaseProceso fase)
    {
        return fase is FaseProceso.RecepcionValidacionInicial 
            or FaseProceso.EstudioMercadoCotizacion;
    }

    /// <summary>
    /// Determina si la fase requiere intervención del Comité CAA.
    /// </summary>
    public static bool RequiereIntervencionCAA(this FaseProceso fase)
    {
        return fase == FaseProceso.AutorizacionCAA;
    }

    /// <summary>
    /// Determina si la fase requiere validación externa (SAT).
    /// Referencia: Sección 14 - Módulo 10 (Validación Fiscal: Panel CFDI).
    /// </summary>
    public static bool RequiereValidacionExterna(this FaseProceso fase)
    {
        return fase == FaseProceso.ValidacionFiscalCFDI;
    }

    /// <summary>
    /// Determina si la fase es de acceso restringido (datos financieros sensibles).
    /// </summary>
    public static bool EsFaseRestringida(this FaseProceso fase)
    {
        return fase is FaseProceso.ValidacionFiscalCFDI or FaseProceso.Pago;
    }

    #endregion

    #region UI/UX - Colores e Iconos

    /// <summary>
    /// Obtiene el color hexadecimal asociado a la fase para UI.
    /// Referencia: Apéndice A - Sistema de Colores por Fase.
    /// </summary>
    public static string ObtenerColorHex(this FaseProceso fase)
    {
        return fase switch
        {
            FaseProceso.RecepcionValidacionInicial => "#0078D4",
            FaseProceso.AutorizacionCAA => "#FFB900",
            FaseProceso.EstudioMercadoCotizacion => "#00B7C3",
            FaseProceso.SeleccionProveedorPedido => "#FF8C00",
            FaseProceso.EntregaBienesServicios => "#6CCB5F",
            FaseProceso.ValidacionFiscalCFDI => "#8764B8",
            FaseProceso.Pago => "#107C10",
            FaseProceso.CierreExpediente => "#69797E",
            _ => "#69797E"
        };
    }

    /// <summary>
    /// Obtiene el color semántico asociado a la fase para componentes WinUI 3.
    /// Valores: Success, Critical, Caution, Attention, Neutral.
    /// </summary>
    public static string ObtenerColorSemantic(this FaseProceso fase)
    {
        return fase switch
        {
            FaseProceso.RecepcionValidacionInicial => "Attention",
            FaseProceso.AutorizacionCAA => "Caution",
            FaseProceso.EstudioMercadoCotizacion => "Attention",
            FaseProceso.SeleccionProveedorPedido => "Caution",
            FaseProceso.EntregaBienesServicios => "Caution",
            FaseProceso.ValidacionFiscalCFDI => "Caution",
            FaseProceso.Pago => "Attention",
            FaseProceso.CierreExpediente => "Success",
            _ => "Neutral"
        };
    }

    /// <summary>
    /// Obtiene el ícono Segoe Fluent Icons asociado a la fase para UI.
    /// </summary>
    public static string ObtenerIcono(this FaseProceso fase)
    {
        return fase switch
        {
            FaseProceso.RecepcionValidacionInicial => "\uE79D",  // Search
            FaseProceso.AutorizacionCAA => "\uE724",             // Forward
            FaseProceso.EstudioMercadoCotizacion => "\uE723",    // Page
            FaseProceso.SeleccionProveedorPedido => "\uE734",    // People
            FaseProceso.EntregaBienesServicios => "\uE806",      // Truck
            FaseProceso.ValidacionFiscalCFDI => "\uE8F0",        // Document
            FaseProceso.Pago => "\uE775",                        // Money
            FaseProceso.CierreExpediente => "\uE74E",            // Folder
            _ => "\uE783"
        };
    }

    /// <summary>
    /// Obtiene el nombre corto de la fase para mostrar en espacios reducidos.
    /// </summary>
    public static string ObtenerNombreCorto(this FaseProceso fase)
    {
        return fase switch
        {
            FaseProceso.RecepcionValidacionInicial => "Recepción",
            FaseProceso.AutorizacionCAA => "CAA",
            FaseProceso.EstudioMercadoCotizacion => "Cotización",
            FaseProceso.SeleccionProveedorPedido => "Selección",
            FaseProceso.EntregaBienesServicios => "Entrega",
            FaseProceso.ValidacionFiscalCFDI => "CFDI",
            FaseProceso.Pago => "Pago",
            FaseProceso.CierreExpediente => "Cierre",
            _ => "N/A"
        };
    }

    #endregion

    #region Roles y Responsables

    /// <summary>
    /// Obtiene el rol principal responsable de la fase.
    /// </summary>
    public static RolAplicacion ObtenerRolResponsable(this FaseProceso fase)
    {
        return fase switch
        {
            FaseProceso.RecepcionValidacionInicial => RolAplicacion.CompradorDSA,
            FaseProceso.AutorizacionCAA => RolAplicacion.RevisorCAA,
            FaseProceso.EstudioMercadoCotizacion => RolAplicacion.CompradorDSA,
            FaseProceso.SeleccionProveedorPedido => RolAplicacion.CompradorDSA,
            FaseProceso.EntregaBienesServicios => RolAplicacion.Almacen,
            FaseProceso.ValidacionFiscalCFDI => RolAplicacion.Finanzas,
            FaseProceso.Pago => RolAplicacion.Finanzas,
            FaseProceso.CierreExpediente => RolAplicacion.CompradorDSA,
            _ => RolAplicacion.ConsultaDSA
        };
    }

    /// <summary>
    /// Obtiene los roles que pueden realizar acciones en la fase.
    /// </summary>
    public static IEnumerable<RolAplicacion> ObtenerRolesActivos(this FaseProceso fase)
    {
        return fase switch
        {
            FaseProceso.RecepcionValidacionInicial => new[] 
            { 
                RolAplicacion.CompradorDSA, 
                RolAplicacion.Administrador 
            },
            FaseProceso.AutorizacionCAA => new[] 
            { 
                RolAplicacion.RevisorCAA, 
                RolAplicacion.Administrador 
            },
            FaseProceso.EstudioMercadoCotizacion => new[] 
            { 
                RolAplicacion.CompradorDSA, 
                RolAplicacion.Administrador 
            },
            FaseProceso.SeleccionProveedorPedido => new[] 
            { 
                RolAplicacion.CompradorDSA, 
                RolAplicacion.Administrador 
            },
            FaseProceso.EntregaBienesServicios => new[] 
            { 
                RolAplicacion.Almacen, 
                RolAplicacion.CompradorDSA,
                RolAplicacion.Administrador 
            },
            FaseProceso.ValidacionFiscalCFDI => new[] 
            { 
                RolAplicacion.Finanzas, 
                RolAplicacion.Administrador 
            },
            FaseProceso.Pago => new[] 
            { 
                RolAplicacion.Finanzas, 
                RolAplicacion.Administrador 
            },
            FaseProceso.CierreExpediente => new[] 
            { 
                RolAplicacion.CompradorDSA, 
                RolAplicacion.Administrador 
            },
            _ => Array.Empty<RolAplicacion>()
        };
    }

    /// <summary>
    /// Determina si un rol específico puede actuar en la fase.
    /// </summary>
    public static bool RolPuedeActuar(this FaseProceso fase, RolAplicacion rol)
    {
        return fase.ObtenerRolesActivos().Contains(rol);
    }

    #endregion

    #region Hitos

    /// <summary>
    /// Obtiene los tipos de hito asociados a esta fase.
    /// Referencia: Sección 11 - Módulo 07 (Timeline de Hitos).
    /// </summary>
    public static IEnumerable<TipoHito> ObtenerHitos(this FaseProceso fase)
    {
        return TipoHitoExtensions.ObtenerHitosPorFase(fase);
    }

    /// <summary>
    /// Obtiene el número de hitos esperados en la fase.
    /// </summary>
    public static int ObtenerNumeroHitosEsperados(this FaseProceso fase)
    {
        return fase switch
        {
            FaseProceso.RecepcionValidacionInicial => 3,
            FaseProceso.AutorizacionCAA => 4,
            FaseProceso.EstudioMercadoCotizacion => 4,
            FaseProceso.SeleccionProveedorPedido => 3,
            FaseProceso.EntregaBienesServicios => 3,
            FaseProceso.ValidacionFiscalCFDI => 4,
            FaseProceso.Pago => 3,
            FaseProceso.CierreExpediente => 3,
            _ => 0
        };
    }

    #endregion
}
