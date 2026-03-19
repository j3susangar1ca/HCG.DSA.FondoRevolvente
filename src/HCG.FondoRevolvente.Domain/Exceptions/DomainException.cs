namespace HCG.FondoRevolvente.Domain.Exceptions;

/// <summary>
/// Excepción base para todas las excepciones del dominio.
/// Representa violaciones a las reglas de negocio o invariantes del dominio.
/// </summary>
public class DomainException : Exception
{
    /// <summary>
    /// Código de error para identificación programática.
    /// </summary>
    public string CodigoError { get; }

    /// <summary>
    /// Datos adicionales contextuales del error.
    /// </summary>
    public Dictionary<string, object> DatosAdicionales { get; }

    public DomainException(string mensaje) : base(mensaje)
    {
        CodigoError = "DOMAIN_ERROR";
        DatosAdicionales = new Dictionary<string, object>();
    }

    public DomainException(string codigoError, string mensaje) : base(mensaje)
    {
        CodigoError = codigoError;
        DatosAdicionales = new Dictionary<string, object>();
    }

    public DomainException(string mensaje, Exception? innerException)
        : base(mensaje, innerException)
    {
        CodigoError = "DOMAIN_ERROR";
        DatosAdicionales = new Dictionary<string, object>();
    }

    public DomainException(string codigoError, string mensaje, Exception? innerException)
        : base(mensaje, innerException)
    {
        CodigoError = codigoError;
        DatosAdicionales = new Dictionary<string, object>();
    }

    /// <summary>
    /// Agrega datos contextuales a la excepción.
    /// </summary>
    public DomainException ConDatos(string clave, object valor)
    {
        DatosAdicionales[clave] = valor;
        return this;
    }
}
