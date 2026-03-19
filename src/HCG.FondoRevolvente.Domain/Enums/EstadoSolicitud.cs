namespace HCG.FondoRevolvente.Domain.Enums;

/// <summary>
/// Representa los estados posibles por los que puede transitar una solicitud.
/// §Módulo 04 y §Módulo 06 README.
/// </summary>
public enum EstadoSolicitud
{
    /// <summary>Borrador inicial, en captura por el solicitante.</summary>
    Borrador,

    /// <summary>Captura completa, pendiente de validación de CFDI y documentos.</summary>
    PendienteValidacion,

    /// <summary>CFDI validado con éxito ante el SAT. Lista para revisión técnica.</summary>
    Validada,

    /// <summary>Rechazada por incumplimiento de requisitos o datos erróneos.</summary>
    Rechazada,

    /// <summary>Enviada a Finanzas/Tesorería para el trámite de pago.</summary>
    EnTramitePago,

    /// <summary>Pago efectuado al proveedor, solicitud finalizada.</summary>
    Pagada,

    /// <summary>Error crítico en comunicación con el SAT, requiere reintento manual o automático.</summary>
    ErrorSatReintentando
}
