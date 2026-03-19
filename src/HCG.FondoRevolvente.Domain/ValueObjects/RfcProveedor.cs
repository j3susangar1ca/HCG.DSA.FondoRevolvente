using System.Text.RegularExpressions;
using HCG.FondoRevolvente.Domain.Exceptions;

namespace HCG.FondoRevolvente.Domain.ValueObjects;

/// <summary>
/// Encapsula el Registro Federal de Contribuyentes (RFC) de un proveedor.
/// Valida el formato según las reglas del SAT México.
/// Soporta tanto Personas Físicas (13 caracteres) como Personas Morales (12 caracteres).
/// </summary>
public sealed record RfcProveedor
{
    // Regex compiladas para validación de RFC según especificaciones del SAT
    // Persona Moral: 12 caracteres - AAA######XXX
    private static readonly Regex _formatoPersonaMoral =
        new(@"^([A-ZÑ&]{3})((\d{2})(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01]))([A-Z0-9]{3})$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    // Persona Física: 13 caracteres - AAAA######XXX
    private static readonly Regex _formatoPersonaFisica =
        new(@"^([A-ZÑ&]{4})((\d{2})(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01]))([A-Z0-9]{3})$",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

    // RFC genéricos que no son válidos para facturación real
    private static readonly HashSet<string> _rfcGenericos = new(StringComparer.OrdinalIgnoreCase)
    {
        "XAXX010101000", // RFC genérico para extranjeros
        "XEXX010101000"  // RFC genérico para extranjeros sin RFC
    };

    /// <summary>
    /// Valor del RFC en mayúsculas.
    /// </summary>
    public string Valor { get; }

    /// <summary>
    /// Indica si el RFC pertenece a una persona física (13 caracteres).
    /// </summary>
    public bool EsPersonaFisica => Valor.Length == 13;

    /// <summary>
    /// Indica si el RFC pertenece a una persona moral (12 caracteres).
    /// </summary>
    public bool EsPersonaMoral => Valor.Length == 12;

    /// <summary>
    /// Razón social o nombre del contribuyente (primeros caracteres del RFC).
    /// </summary>
    public string Siglas => EsPersonaFisica ? Valor[..4] : Valor[..3];

    /// <summary>
    /// Fecha de constitución o nacimiento extraída del RFC (puede no ser exacta).
    /// </summary>
    public DateOnly? FechaDeclarada
    {
        get
        {
            var fechaStr = EsPersonaFisica ? Valor.Substring(4, 6) : Valor.Substring(3, 6);
            if (fechaStr == "000000") return null;

            // El año en el RFC usa 2 dígitos, asumimos el siglo más probable
            var anio = int.Parse(fechaStr[..2]);
            anio += anio <= 30 ? 2000 : 1900; // Ajuste de siglo

            var mes = int.Parse(fechaStr.Substring(2, 2));
            var dia = int.Parse(fechaStr.Substring(4, 2));

            if (mes is < 1 or > 12) return null;
            if (dia is < 1 or > 31) return null;

            try
            {
                return new DateOnly(anio, mes, Math.Min(dia, DateTime.DaysInMonth(anio, mes)));
            }
            catch
            {
                return null;
            }
        }
    }

    /// <summary>
    /// Homoclave del RFC (últimos 3 caracteres).
    /// </summary>
    public string Homoclave => Valor[^3..];

    private RfcProveedor(string valor)
    {
        Valor = valor.ToUpperInvariant();
    }

    #region Factory Methods

    /// <summary>
    /// Crea una instancia validada de <see cref="RfcProveedor"/>.
    /// </summary>
    /// <param name="rfc">RFC del proveedor (12 o 13 caracteres).</param>
    /// <exception cref="DomainException">Si el formato del RFC es inválido.</exception>
    public static RfcProveedor Crear(string rfc)
    {
        if (string.IsNullOrWhiteSpace(rfc))
            throw new DomainException("El RFC del proveedor no puede estar vacío.");

        var rfcLimpio = rfc.Trim().ToUpperInvariant().Replace("-", "").Replace(" ", "");

        if (rfcLimpio.Length is not (12 or 13))
            throw new DomainException(
                $"El RFC debe tener 12 caracteres (persona moral) o 13 caracteres (persona física). " +
                $"Longitud recibida: {rfcLimpio.Length}.");

        // Validar formato según tipo de persona
        var esValido = rfcLimpio.Length == 12
            ? _formatoPersonaMoral.IsMatch(rfcLimpio)
            : _formatoPersonaFisica.IsMatch(rfcLimpio);

        if (!esValido)
            throw new DomainException(
                $"El RFC '{MaskRfc(rfcLimpio)}' no tiene un formato válido según las reglas del SAT.");

        // Advertencia para RFC genéricos (no lanzamos excepción, pero se registra)
        if (_rfcGenericos.Contains(rfcLimpio))
            throw new DomainException(
                $"El RFC '{rfcLimpio}' es un RFC genérico que no es válido para proveedores registrados.");

        return new RfcProveedor(rfcLimpio);
    }

    /// <summary>
    /// Intenta crear una instancia sin lanzar excepción.
    /// </summary>
    public static bool TryCrear(string? rfc, out RfcProveedor? rfcProveedor, out string? error)
    {
        rfcProveedor = null;
        error = null;

        if (string.IsNullOrWhiteSpace(rfc))
        {
            error = "El RFC no puede estar vacío.";
            return false;
        }

        var rfcLimpio = rfc.Trim().ToUpperInvariant().Replace("-", "").Replace(" ", "");

        if (rfcLimpio.Length is not (12 or 13))
        {
            error = $"El RFC debe tener 12 o 13 caracteres. Longitud: {rfcLimpio.Length}.";
            return false;
        }

        var esValido = rfcLimpio.Length == 12
            ? _formatoPersonaMoral.IsMatch(rfcLimpio)
            : _formatoPersonaFisica.IsMatch(rfcLimpio);

        if (!esValido)
        {
            error = "El formato del RFC no es válido.";
            return false;
        }

        if (_rfcGenericos.Contains(rfcLimpio))
        {
            error = "El RFC genérico no es válido para proveedores registrados.";
            return false;
        }

        rfcProveedor = new RfcProveedor(rfcLimpio);
        return true;
    }

    /// <summary>
    /// Reconstituye un RfcProveedor desde su valor almacenado (sin validación completa).
    /// Usado por el repositorio al cargar desde la base de datos.
    /// </summary>
    public static RfcProveedor DesdeValorAlmacenado(string valor)
    {
        if (string.IsNullOrWhiteSpace(valor))
            throw new DomainException("El RFC almacenado no puede estar vacío.");

        return new RfcProveedor(valor.Trim().ToUpperInvariant());
    }

    #endregion

    #region Métodos de Dominio

    /// <summary>
    /// Determina si este RFC pertenece al mismo contribuyente que otro RFC.
    /// Útil para detectar fraccionamiento (RN-002).
    /// </summary>
    public bool EsMismoContribuyente(RfcProveedor otro) =>
        string.Equals(Valor, otro.Valor, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Enmascara el RFC para mostrar en logs o UI (protección de datos sensibles).
    /// Ejemplo: ABCD123456XXX -> ABCD****XXX
    /// </summary>
    public string Enmascarado() => MaskRfc(Valor);

    #endregion

    #region Helpers Privados

    private static string MaskRfc(string rfc) =>
        rfc.Length <= 7 ? "****" : $"{rfc[..4]}****{rfc[^3..]}";

    #endregion

    #region Presentación

    /// <summary>
    /// Devuelve el RFC formateado con guiones para visualización.
    /// Ejemplo: ABCD-123456-XXX
    /// </summary>
    public string FormatoVisual() => EsPersonaFisica
        ? $"{Valor[..4]}-{Valor.Substring(4, 6)}-{Valor[^3..]}"
        : $"{Valor[..3]}-{Valor.Substring(3, 6)}-{Valor[^3..]}";

    public override string ToString() => Valor;

    #endregion
}
