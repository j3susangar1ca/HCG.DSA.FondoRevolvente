namespace HCG.FondoRevolvente.Domain.Enums;

/// <summary>
/// Roles de usuario dentro del sistema, determinan permisos y visibilidad de datos (PII/Fiscal).
/// §3.1 y §3.2 README.
/// </summary>
public enum RolAplicacion
{
    /// <summary>Acceso total: configuración, auditoría y gestión de usuarios (§Módulo 13).</summary>
    Administrador = 1,

    /// <summary>Crea solicitudes, sube cotizaciones y gestiona su propio flujo operativo.</summary>
    CompradorDSA = 2,

    /// <summary>Miembro del Comité de Adquisiciones y Arrendamientos. Autoriza solicitudes de alto monto.</summary>
    RevisorCAA = 3,

    /// <summary>Gestiona los trámites de pago y tiene visibilidad total de datos fiscales.</summary>
    Finanzas = 4,

    /// <summary>Recibe bienes y servicios en el almacén del hospital.</summary>
    Almacen = 5,

    /// <summary>Acceso de solo lectura a todos los expedientes para fines de inspección.</summary>
    ConsultaDSA = 6
}
