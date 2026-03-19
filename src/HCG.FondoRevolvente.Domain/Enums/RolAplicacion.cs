using System.ComponentModel;
using System.Runtime.Serialization;

namespace HCG.FondoRevolvente.Domain.Enums;

/// <summary>
/// Define los 6 roles funcionales del Sistema de Gestión de Fondo Revolvente.
/// <para>
/// Referencia: Sección 3.1 - Grupos de Seguridad y Roles de la Aplicación.
/// Referencia: Apéndice B - Matriz de Visibilidad de Módulos por Rol.
/// </para>
/// <remarks>
/// La autenticación se realiza contra Active Directory del hospital.
/// Los grupos de AD se mapean a estos roles de aplicación mediante claims JWT.
/// Implementa el principio de mínimo privilegio visual en la interfaz (Sección 3.2).
/// </remarks>
/// </summary>
public enum RolAplicacion
{
    /// <summary>
    /// Administrador del Sistema.
    /// <para>Grupo AD: Administradores_Sistema</para>
    /// <para>Permisos: Acceso completo a todos los módulos, acciones y datos del sistema.</para>
    /// <para>Puede gestionar usuarios, roles y configuración del sistema.</para>
    /// </summary>
    [Description("Administrador")]
    [EnumMember(Value = "ADMINISTRADOR")]
    Administrador = 1,

    /// <summary>
    /// Comprador del Departamento de Servicios Administrativos (DSA).
    /// <para>Grupo AD: Compradores_DSA</para>
    /// <para>Permisos: Creación y gestión de solicitudes propias.</para>
    /// <para>Puede gestionar proveedores, cotizaciones y seguimiento de expedientes.</para>
    /// <para>Filtra el DataGrid para ver únicamente sus propias solicitudes.</para>
    /// </summary>
    [Description("Comprador DSA")]
    [EnumMember(Value = "COMPRADOR_DSA")]
    CompradorDSA = 2,

    /// <summary>
    /// Miembro del Comité de Adquisiciones y Arrendamientos (CAA).
    /// <para>Grupo AD: CAA_Miembros</para>
    /// <para>Permisos: Consulta de expedientes en estado de autorización CAA.</para>
    /// <para>Puede autorizar o rechazar solicitudes. Firma digital de autorizaciones.</para>
    /// </summary>
    [Description("Revisor CAA")]
    [EnumMember(Value = "REVISOR_CAA")]
    RevisorCAA = 3,

    /// <summary>
    /// Personal de Recursos Financieros.
    /// <para>Grupo AD: Recursos_Financieros</para>
    /// <para>Permisos: Gestión de hitos de validación fiscal (CFDI 4.0).</para>
    /// <para>Proceso de pago y generación de complementos de pago.</para>
    /// <para>Acceso a datos financieros detallados y RFC completos.</para>
    /// </summary>
    [Description("Recursos Financieros")]
    [EnumMember(Value = "FINANZAS")]
    Finanzas = 4,

    /// <summary>
    /// Personal de Almacén.
    /// <para>Grupo AD: Almacen_Staff</para>
    /// <para>Permisos: Confirmación de hitos de entrega y recepción.</para>
    /// <para>Recepción de bienes y validación de especificaciones.</para>
    /// <para>Gestión de actas de recepción.</para>
    /// </summary>
    [Description("Almacén")]
    [EnumMember(Value = "ALMACEN")]
    Almacen = 5,

    /// <summary>
    /// Personal DSA con acceso de solo lectura.
    /// <para>Grupo AD: DSA_Staff</para>
    /// <para>Permisos: Acceso de solo lectura a solicitudes, reportes y dashboard.</para>
    /// <para>Sin capacidad de modificación o creación de registros.</para>
    /// </summary>
    [Description("Consulta DSA")]
    [EnumMember(Value = "CONSULTA_DSA")]
    ConsultaDSA = 6
}

/// <summary>
/// Permisos granulares del sistema para control de acceso fino.
/// Se combinan con roles para determinar autorización final.
/// Referencia: Apéndice B - Matriz de Visibilidad de Módulos por Rol.
/// </summary>
[Flags]
public enum PermisoSistema : long
{
    /// <summary>Sin permisos asignados.</summary>
    Ninguno = 0,

    #region Permisos de Solicitud

    /// <summary>Puede crear nuevas solicitudes.</summary>
    CrearSolicitud = 1L << 0,

    /// <summary>Puede editar solicitudes existentes.</summary>
    EditarSolicitud = 1L << 1,

    /// <summary>Puede eliminar solicitudes (solo estados iniciales).</summary>
    EliminarSolicitud = 1L << 2,

    /// <summary>Puede ver solicitudes propias.</summary>
    VerSolicitudPropia = 1L << 3,

    /// <summary>Puede ver todas las solicitudes del sistema.</summary>
    VerSolicitudTodas = 1L << 4,

    /// <summary>Puede autorizar solicitudes en estado CAA.</summary>
    AutorizarSolicitud = 1L << 5,

    /// <summary>Puede rechazar solicitudes.</summary>
    RechazarSolicitud = 1L << 6,

    /// <summary>Puede transicionar estados de solicitud.</summary>
    TransicionarEstado = 1L << 7,

    #endregion

    #region Permisos de Cotización

    /// <summary>Puede agregar cotizaciones a solicitudes.</summary>
    AgregarCotizacion = 1L << 8,

    /// <summary>Puede editar cotizaciones existentes.</summary>
    EditarCotizacion = 1L << 9,

    /// <summary>Puede eliminar cotizaciones.</summary>
    EliminarCotizacion = 1L << 10,

    /// <summary>Puede seleccionar proveedor ganador.</summary>
    SeleccionarProveedor = 1L << 11,

    /// <summary>Puede ver cuadro comparativo.</summary>
    VerCuadroComparativo = 1L << 12,

    /// <summary>Puede generar cuadro comparativo PDF.</summary>
    GenerarCuadroComparativo = 1L << 13,

    #endregion

    #region Permisos de Proveedor

    /// <summary>Puede crear nuevos proveedores.</summary>
    CrearProveedor = 1L << 14,

    /// <summary>Puede editar proveedores existentes.</summary>
    EditarProveedor = 1L << 15,

    /// <summary>Puede eliminar proveedores.</summary>
    EliminarProveedor = 1L << 16,

    /// <summary>Puede ver información de proveedores.</summary>
    VerProveedor = 1L << 17,

    /// <summary>Puede ver RFC completo de proveedores (dato sensible).</summary>
    VerRfcCompleto = 1L << 18,

    #endregion

    #region Permisos de Validación Fiscal

    /// <summary>Puede validar CFDI ante el SAT.</summary>
    ValidarCfdi = 1L << 19,

    /// <summary>Puede ver datos fiscales detallados.</summary>
    VerDatosFiscales = 1L << 20,

    /// <summary>Puede cargar archivos XML de CFDI.</summary>
    CargarCfdi = 1L << 21,

    #endregion

    #region Permisos de Pago

    /// <summary>Puede procesar pagos a proveedores.</summary>
    ProcesarPago = 1L << 22,

    /// <summary>Puede generar complementos de pago.</summary>
    GenerarComplemento = 1L << 23,

    /// <summary>Puede ver información bancaria.</summary>
    VerInfoBancaria = 1L << 24,

    #endregion

    #region Permisos de Recepción

    /// <summary>Puede registrar recepción de bienes.</summary>
    RegistrarRecepcion = 1L << 25,

    /// <summary>Puede rechazar recepciones por no conformidad.</summary>
    RechazarRecepcion = 1L << 26,

    /// <summary>Puede generar actas de recepción.</summary>
    GenerarActaRecepcion = 1L << 27,

    #endregion

    #region Permisos de Reportes

    /// <summary>Puede generar reportes.</summary>
    GenerarReportes = 1L << 28,

    /// <summary>Puede exportar datos a Excel/PDF.</summary>
    ExportarDatos = 1L << 29,

    /// <summary>Puede ver reportes de auditoría.</summary>
    VerAuditoria = 1L << 30,

    #endregion

    #region Permisos de Dashboard

    /// <summary>Puede ver dashboard general.</summary>
    VerDashboard = 1L << 31,

    /// <summary>Puede ver KPIs detallados.</summary>
    VerKpiDetallados = 1L << 32,

    /// <summary>Puede ver métricas financieras.</summary>
    VerMetricasFinancieras = 1L << 33

    #endregion
}

/// <summary>
/// Métodos de extensión para el enum RolAplicacion.
/// Proporciona utilidades para autorización, UI y mapeo con Active Directory.
/// </summary>
public static class RolAplicacionExtensions
{
    #region Mapeo Active Directory

    /// <summary>
    /// Obtiene el nombre del grupo de Active Directory asociado al rol.
    /// </summary>
    public static string ObtenerGrupoActiveDirectory(this RolAplicacion rol)
    {
        return rol switch
        {
            RolAplicacion.Administrador => "Administradores_Sistema",
            RolAplicacion.CompradorDSA => "Compradores_DSA",
            RolAplicacion.RevisorCAA => "CAA_Miembros",
            RolAplicacion.Finanzas => "Recursos_Financieros",
            RolAplicacion.Almacen => "Almacen_Staff",
            RolAplicacion.ConsultaDSA => "DSA_Staff",
            _ => throw new ArgumentOutOfRangeException(nameof(rol))
        };
    }

    /// <summary>
    /// Intenta mapear un nombre de grupo AD a un rol de aplicación.
    /// </summary>
    public static bool TryParseFromGrupoAD(string grupoAD, out RolAplicacion rol)
    {
        rol = grupoAD?.ToUpperInvariant() switch
        {
            "ADMINISTRADORES_SISTEMA" => RolAplicacion.Administrador,
            "COMPRADORES_DSA" => RolAplicacion.CompradorDSA,
            "CAA_MIEMBROS" => RolAplicacion.RevisorCAA,
            "RECURSOS_FINANCIEROS" => RolAplicacion.Finanzas,
            "ALMACEN_STAFF" => RolAplicacion.Almacen,
            "DSA_STAFF" => RolAplicacion.ConsultaDSA,
            _ => default
        };

        return rol != default;
    }

    #endregion

    #region Permisos

    /// <summary>
    /// Obtiene los permisos asociados a un rol específico.
    /// </summary>
    public static PermisoSistema ObtenerPermisos(this RolAplicacion rol)
    {
        return rol switch
        {
            RolAplicacion.Administrador => 
                PermisoSistema.CrearSolicitud | PermisoSistema.EditarSolicitud | 
                PermisoSistema.EliminarSolicitud | PermisoSistema.VerSolicitudPropia | 
                PermisoSistema.VerSolicitudTodas | PermisoSistema.AutorizarSolicitud |
                PermisoSistema.RechazarSolicitud | PermisoSistema.TransicionarEstado |
                PermisoSistema.AgregarCotizacion | PermisoSistema.EditarCotizacion |
                PermisoSistema.EliminarCotizacion | PermisoSistema.SeleccionarProveedor |
                PermisoSistema.VerCuadroComparativo | PermisoSistema.GenerarCuadroComparativo |
                PermisoSistema.CrearProveedor | PermisoSistema.EditarProveedor |
                PermisoSistema.EliminarProveedor | PermisoSistema.VerProveedor |
                PermisoSistema.VerRfcCompleto | PermisoSistema.ValidarCfdi |
                PermisoSistema.VerDatosFiscales | PermisoSistema.CargarCfdi |
                PermisoSistema.ProcesarPago | PermisoSistema.GenerarComplemento |
                PermisoSistema.VerInfoBancaria | PermisoSistema.RegistrarRecepcion |
                PermisoSistema.RechazarRecepcion | PermisoSistema.GenerarActaRecepcion |
                PermisoSistema.GenerarReportes | PermisoSistema.ExportarDatos |
                PermisoSistema.VerAuditoria | PermisoSistema.VerDashboard |
                PermisoSistema.VerKpiDetallados | PermisoSistema.VerMetricasFinancieras,

            RolAplicacion.CompradorDSA => 
                PermisoSistema.CrearSolicitud | PermisoSistema.EditarSolicitud |
                PermisoSistema.VerSolicitudPropia | PermisoSistema.TransicionarEstado |
                PermisoSistema.AgregarCotizacion | PermisoSistema.EditarCotizacion |
                PermisoSistema.SeleccionarProveedor | PermisoSistema.VerCuadroComparativo |
                PermisoSistema.GenerarCuadroComparativo | PermisoSistema.CrearProveedor |
                PermisoSistema.EditarProveedor | PermisoSistema.VerProveedor |
                PermisoSistema.CargarCfdi | PermisoSistema.GenerarReportes |
                PermisoSistema.VerDashboard,

            RolAplicacion.RevisorCAA => 
                PermisoSistema.VerSolicitudTodas | PermisoSistema.AutorizarSolicitud |
                PermisoSistema.RechazarSolicitud | PermisoSistema.VerCuadroComparativo |
                PermisoSistema.VerProveedor | PermisoSistema.VerDashboard,

            RolAplicacion.Finanzas => 
                PermisoSistema.VerSolicitudTodas | PermisoSistema.ValidarCfdi |
                PermisoSistema.VerDatosFiscales | PermisoSistema.VerRfcCompleto |
                PermisoSistema.ProcesarPago | PermisoSistema.GenerarComplemento |
                PermisoSistema.VerInfoBancaria | PermisoSistema.GenerarReportes |
                PermisoSistema.ExportarDatos | PermisoSistema.VerDashboard |
                PermisoSistema.VerKpiDetallados | PermisoSistema.VerMetricasFinancieras,

            RolAplicacion.Almacen => 
                PermisoSistema.VerSolicitudTodas | PermisoSistema.RegistrarRecepcion |
                PermisoSistema.RechazarRecepcion | PermisoSistema.GenerarActaRecepcion |
                PermisoSistema.VerDashboard,

            RolAplicacion.ConsultaDSA => 
                PermisoSistema.VerSolicitudPropia | PermisoSistema.VerCuadroComparativo |
                PermisoSistema.VerProveedor | PermisoSistema.GenerarReportes |
                PermisoSistema.VerDashboard,

            _ => PermisoSistema.Ninguno
        };
    }

    /// <summary>
    /// Verifica si un rol tiene un permiso específico.
    /// </summary>
    public static bool TienePermiso(this RolAplicacion rol, PermisoSistema permiso)
    {
        var permisos = rol.ObtenerPermisos();
        return (permisos & permiso) == permiso;
    }

    /// <summary>
    /// Verifica si un rol tiene todos los permisos especificados.
    /// </summary>
    public static bool TieneTodosPermisos(this RolAplicacion rol, params PermisoSistema[] permisos)
    {
        return permisos.All(p => rol.TienePermiso(p));
    }

    /// <summary>
    /// Verifica si un rol tiene al menos uno de los permisos especificados.
    /// </summary>
    public static bool TieneAlgunPermiso(this RolAplicacion rol, params PermisoSistema[] permisos)
    {
        return permisos.Any(p => rol.TienePermiso(p));
    }

    #endregion

    #region Fases y Estados

    /// <summary>
    /// Determina si el rol puede editar solicitudes en una fase específica.
    /// </summary>
    public static bool PuedeEditarEnFase(this RolAplicacion rol, FaseProceso fase)
    {
        if (!rol.TienePermiso(PermisoSistema.EditarSolicitud))
            return false;

        if (rol is RolAplicacion.Administrador or RolAplicacion.CompradorDSA)
        {
            return fase is FaseProceso.RecepcionValidacionInicial 
                or FaseProceso.EstudioMercadoCotizacion;
        }

        return false;
    }

    /// <summary>
    /// Obtiene las fases en las que un rol puede realizar acciones.
    /// </summary>
    public static IEnumerable<FaseProceso> ObtenerFasesActivas(this RolAplicacion rol)
    {
        return rol switch
        {
            RolAplicacion.Administrador => Enum.GetValues<FaseProceso>(),
            
            RolAplicacion.CompradorDSA => new[]
            {
                FaseProceso.RecepcionValidacionInicial,
                FaseProceso.EstudioMercadoCotizacion,
                FaseProceso.SeleccionProveedorPedido,
                FaseProceso.CierreExpediente
            },
            
            RolAplicacion.RevisorCAA => new[] { FaseProceso.AutorizacionCAA },
            RolAplicacion.Finanzas => new[] { FaseProceso.ValidacionFiscalCFDI, FaseProceso.Pago },
            RolAplicacion.Almacen => new[] { FaseProceso.EntregaBienesServicios },
            RolAplicacion.ConsultaDSA => Array.Empty<FaseProceso>(),
            
            _ => Array.Empty<FaseProceso>()
        };
    }

    /// <summary>
    /// Determina si un rol puede actuar en una fase específica.
    /// </summary>
    public static bool PuedeActuarEnFase(this RolAplicacion rol, FaseProceso fase)
    {
        return rol.ObtenerFasesActivas().Contains(fase);
    }

    #endregion

    #region UI/UX

    /// <summary>
    /// Obtiene el ícono Segoe Fluent Icons asociado al rol.
    /// </summary>
    public static string ObtenerIcono(this RolAplicacion rol)
    {
        return rol switch
        {
            RolAplicacion.Administrador => "\uE770",   // Settings
            RolAplicacion.CompradorDSA => "\uE734",    // People
            RolAplicacion.RevisorCAA => "\uE73E",      // Completed
            RolAplicacion.Finanzas => "\uE775",        // Money
            RolAplicacion.Almacen => "\uE806",         // Truck
            RolAplicacion.ConsultaDSA => "\uE721",     // View
            _ => "\uE783"
        };
    }

    /// <summary>
    /// Obtiene el color hexadecimal asociado al rol para UI.
    /// </summary>
    public static string ObtenerColorHex(this RolAplicacion rol)
    {
        return rol switch
        {
            RolAplicacion.Administrador => "#6B69D6",
            RolAplicacion.CompradorDSA => "#0078D4",
            RolAplicacion.RevisorCAA => "#FFB900",
            RolAplicacion.Finanzas => "#107C10",
            RolAplicacion.Almacen => "#FF8C00",
            RolAplicacion.ConsultaDSA => "#69797E",
            _ => "#69797E"
        };
    }

    /// <summary>
    /// Determina si el rol tiene acceso de solo lectura.
    /// </summary>
    public static bool EsSoloLectura(this RolAplicacion rol)
    {
        return rol == RolAplicacion.ConsultaDSA;
    }

    /// <summary>
    /// Determina si el rol puede ver datos financieros sensibles.
    /// </summary>
    public static bool PuedeVerDatosFinancieros(this RolAplicacion rol)
    {
        return rol is RolAplicacion.Administrador or RolAplicacion.Finanzas;
    }

    #endregion

    #region Módulos (Apéndice B)

    /// <summary>
    /// Obtiene los módulos accesibles para el rol.
    /// Referencia: Apéndice B - Matriz de Visibilidad de Módulos por Rol.
    /// </summary>
    public static IEnumerable<int> ObtenerModulosAccesibles(this RolAplicacion rol)
    {
        // Módulos: 1=Auth, 2=Shell, 3=Dashboard, 4=Lista, 5=Nueva, 
        // 6=Detalle, 7=Timeline, 8=Proveedores, 9=Cotizaciones, 
        // 10=CFDI, 11=Reportes, 12=Notificaciones, 13=Admin

        return rol switch
        {
            RolAplicacion.Administrador => 
                new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 },
            
            RolAplicacion.CompradorDSA => 
                new[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 11, 12 },
            
            RolAplicacion.RevisorCAA => 
                new[] { 1, 2, 3, 4, 6, 7, 9, 12 },
            
            RolAplicacion.Finanzas => 
                new[] { 1, 2, 3, 4, 6, 7, 9, 10, 11, 12 },
            
            RolAplicacion.Almacen => 
                new[] { 1, 2, 3, 4, 6, 7, 12 },
            
            RolAplicacion.ConsultaDSA => 
                new[] { 1, 2, 3, 4, 6, 7, 9, 11 },
            
            _ => Array.Empty<int>()
        };
    }

    /// <summary>
    /// Determina si el rol tiene acceso a un módulo específico.
    /// </summary>
    public static bool TieneAccesoModulo(this RolAplicacion rol, int moduloId)
    {
        return rol.ObtenerModulosAccesibles().Contains(moduloId);
    }

    #endregion
}
