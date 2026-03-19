namespace HCG.FondoRevolvente.Domain.Exceptions;

/// <summary>
/// Excepción lanzada cuando la validación de un CFDI ante el SAT falla.
/// Implementa RN-004: Validación de CFDI 4.0 ante el SAT.
/// </summary>
public class CfdiInvalidoException : DomainException
{
    /// <summary>
    /// UUID del CFDI que falló la validación.
    /// </summary>
    public string UuidCfdi { get; }

    /// <summary>
    /// RFC del emisor del CFDI.
    /// </summary>
    public string RfcEmisor { get; }

    /// <summary>
    /// Código de error devuelto por el SAT.
    /// </summary>
    public string? CodigoErrorSat { get; }

    /// <summary>
    /// Mensaje de error devuelto por el SAT.
    /// </summary>
    public string? MensajeErrorSat { get; }

    /// <summary>
    /// Indica si el error es recuperable (se puede reintentar).
    /// </summary>
    public bool EsRecuperable { get; }

    public CfdiInvalidoException(string uuidCfdi, string rfcEmisor, string? codigoError = null, string? mensajeError = null)
        : base(
            "RN004_CFDI_INVALIDO",
            $"El CFDI con UUID {uuidCfdi} del emisor {MaskRfc(rfcEmisor)} no pasó la validación del SAT. " +
            (codigoError is not null ? $"Código de error: {codigoError}. " : string.Empty) +
            (mensajeError is not null ? $"Detalle: {mensajeError}" : string.Empty))
    {
        UuidCfdi = uuidCfdi;
        RfcEmisor = rfcEmisor;
        CodigoErrorSat = codigoError;
        MensajeErrorSat = mensajeError;
        EsRecuperable = false;

        ConDatos("UuidCfdi", uuidCfdi)
            .ConDatos("RfcEmisor", MaskRfc(rfcEmisor))
            .ConDatos("CodigoErrorSat", codigoError ?? "N/A")
            .ConDatos("MensajeErrorSat", mensajeError ?? "N/A")
            .ConDatos("EsRecuperable", false);
    }

    public CfdiInvalidoException(string uuidCfdi, string rfcEmisor, bool esRecuperable, string mensaje)
        : base(
            "RN004_CFDI_VALIDACION_ERROR",
            $"Error al validar CFDI con UUID {uuidCfdi}. {mensaje}")
    {
        UuidCfdi = uuidCfdi;
        RfcEmisor = rfcEmisor;
        EsRecuperable = esRecuperable;

        ConDatos("UuidCfdi", uuidCfdi)
            .ConDatos("RfcEmisor", MaskRfc(rfcEmisor))
            .ConDatos("EsRecuperable", esRecuperable);
    }

    /// <summary>
    /// Enmascara el RFC para mostrar solo los primeros 4 caracteres y los últimos 3.
    /// </summary>
    private static string MaskRfc(string rfc) =>
        rfc.Length <= 7 ? rfc : $"{rfc[..4]}****{rfc[^3..]}";
}
