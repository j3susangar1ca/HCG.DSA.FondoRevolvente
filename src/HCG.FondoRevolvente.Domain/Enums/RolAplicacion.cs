namespace HCG.FondoRevolvente.Domain.Enums;

/// <summary>
/// Roles de usuario dentro del sistema, determinan permisos y visibilidad de datos (PII/Fiscal).
/// §3.1 y §3.2 README.
/// </summary>
public enum RolAplicacion
{
    /// <summary>Acceso total: configuración, auditoría y gestión de usuarios (§Módulo 13).</summary>
    Administrador,

    /// <summary>Crea solicitudes, sube cotizaciones y gestiona su propio flujo operativo.</summary>
    Solicitante,

    /// <summary>Valida CFDI, revisa tech-specs y autoriza técnica/administrativamente.</summary>
    Revisor,

    /// <summary>Gestiona los trámites de pago y tiene visibilidad total de datos fiscales.</summary>
    Finanzas,

    /// <summary>Acceso de solo lectura a todos los expedientes para fines de inspección.</summary>
    Auditor
}
