namespace HCG.FondoRevolvente.Domain.Enums;

/// <summary>
/// Clasifica los tipos de hitos (eventos) registrados en el historial de la solicitud.
/// §Módulo 07 README.
/// </summary>
public enum TipoHito
{
    /// <summary>Creación inicial de la solicitud.</summary>
    Creacion,

    /// <summary>Cambio de estado en el flujo (Borrador -> PendienteValidacion, etc.).</summary>
    CambioEstado,

    /// <summary>Documento adjunto (Cotización, CFDI, etc.).</summary>
    DocumentoAdjunto,

    /// <summary>Resultado de validación fiscal (Exitoso/Fallido).</summary>
    ValidacionFiscal,

    /// <summary>Comentario u observación manual de un revisor.</summary>
    Observacion,

    /// <summary>Bloqueo o liberación de edición del expediente (RN-005).</summary>
    GestionBloqueo
}
