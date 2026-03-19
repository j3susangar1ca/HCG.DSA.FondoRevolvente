using System.Text.RegularExpressions;
using HCG.FondoRevolvente.Domain.Exceptions;

namespace HCG.FondoRevolvente.Domain.ValueObjects;

/// <summary>
/// Encapsula el RFC de un proveedor con validación estructural del formato SAT.
/// 
/// Personas morales:  AAA######XX   → 12 caracteres (3 letras + 6 dígitos + 3 alfanumérico)
/// Personas físicas:  AAAA######XXX → 13 caracteres (4 letras + 6 dígitos + 3 alfanumérico)
/// 
/// Almacenado cifrado en BD (§RfcEncryptionService).
/// Presentado parcialmente enmascarado según rol: AAAA######*** (§3.2 README).
/// </summary>
public sealed record RfcProveedor
{
    // SAT estructura: letras iniciales + fecha YYMMDD + homoclave
    // Moral:   ^[A-ZÑ&]{3}\d{6}[A-Z\d]{3}$
    // Física:  ^[A-ZÑ&]{4}\d{6}[A-Z\d]{3}$
    private static readonly Regex _moral =
        new(@"^[A-ZÑ&]{3}\d{6}[A-Z\d]{3}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    private static readonly Regex _fisica =
        new(@"^[A-ZÑ&]{4}\d{6}[A-Z\d]{3}$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

    // RFCs genéricos del SAT usados para operaciones especiales — siempre válidos
    private static readonly IReadOnlySet<string> _rfcsGenericos = new HashSet<string>(
        StringComparer.OrdinalIgnoreCase)
    {
        "XAXX010101000", // Público en general
        "XEXX010101000"  // Extranjero
    };

    /// <summary>RFC completo, normalizado a mayúsculas. Almacenado cifrado.</summary>
    public string Valor { get; }

    /// <summary>Tipo de persona según la longitud del RFC.</summary>
    public TipoPersonaRfc TipoPersona { get; }

    private RfcProveedor(string valor)
    {
        Valor = valor;
        TipoPersona = valor.Length == 12 ? TipoPersonaRfc.Moral : TipoPersonaRfc.Fisica;
    }

    // -------------------------------------------------------------------------
    // Factory Methods
    // -------------------------------------------------------------------------

    /// <summary>
    /// Crea una instancia con validación estructural completa del formato SAT.
    /// </summary>
    /// <param name="rfc">RFC en cualquier capitalización. Se normaliza internamente.</param>
    /// <exception cref="DomainException">Si el formato no es RFC mexicano válido.</exception>
    public static RfcProveedor Crear(string rfc)
    {
        if (string.IsNullOrWhiteSpace(rfc))
            throw new DomainException("El RFC del proveedor no puede estar vacío.");

        var normalizado = Normalizar(rfc);

        // Permitir RFCs genéricos del SAT
        if (_rfcsGenericos.Contains(normalizado))
            return new RfcProveedor(normalizado);

        if (!EsEstructuralmenteValido(normalizado, out var mensaje))
            throw new DomainException(mensaje);

        return new RfcProveedor(normalizado);
    }

    /// <summary>
    /// Valida sin lanzar excepción.
    /// Usado para validación carácter a carácter en el TextBox de registro (§Módulo 08).
    /// </summary>
    /// <param name="rfc">RFC a validar (acepta entrada parcial).</param>
    /// <param name="estadoValidacion">Estado actual para controlar el color del borde en la UI.</param>
    public static EstadoValidacionRfc ValidarProgresivamente(string? rfc)
    {
        if (string.IsNullOrEmpty(rfc))
            return EstadoValidacionRfc.Vacio;

        var normalizado = Normalizar(rfc);
        var longitud = normalizado.Length;

        // Durante la escritura: guiar al usuario sin penalizarlo prematuramente
        if (longitud < 12)
            return EstadoValidacionRfc.Parcial;

        if (_rfcsGenericos.Contains(normalizado))
            return EstadoValidacionRfc.Valido;

        return EsEstructuralmenteValido(normalizado, out _)
            ? EstadoValidacionRfc.Valido
            : EstadoValidacionRfc.Invalido;
    }

    // -------------------------------------------------------------------------
    // Enmascaramiento por rol — §3.2 "Principio de Mínimo Privilegio"
    // -------------------------------------------------------------------------

    /// <summary>
    /// RFC completo — solo para roles con acceso fiscal (Administrador, Finanzas).
    /// Ejemplo: ABCD860512XYZ
    /// </summary>
    public string ObtenerCompleto() => Valor;

    /// <summary>
    /// RFC enmascarado para roles sin acceso fiscal completo.
    /// Expone las iniciales y la fecha, oculta la homoclave.
    /// Ejemplo: ABCD860512*** (§Módulo 08, Módulo 09 README).
    /// </summary>
    public string ObtenerEnmascarado()
    {
        // Preserva todo menos la homoclave (últimos 3 caracteres)
        var visibles = Valor[..^3];
        return visibles + "***";
    }

    /// <summary>
    /// Devuelve la representación adecuada según el rol del usuario.
    /// Centraliza la lógica de enmascaramiento — nunca depender del ViewModel para esto.
    /// </summary>
    public string ObtenerSegunRol(bool tieneAccesoFiscal) =>
        tieneAccesoFiscal ? ObtenerCompleto() : ObtenerEnmascarado();

    // -------------------------------------------------------------------------
    // Identidad de negocio
    // -------------------------------------------------------------------------

    /// <summary>
    /// Compara RFCs sin importar capitalización.
    /// Crítico para verificar la coincidencia RFC emisor ↔ proveedor en validación CFDI (§Módulo 10).
    /// </summary>
    public bool CoincideCon(string rfcAComparar) =>
        string.Equals(Valor, Normalizar(rfcAComparar), StringComparison.OrdinalIgnoreCase);

    // -------------------------------------------------------------------------
    // Helpers privados
    // -------------------------------------------------------------------------

    private static string Normalizar(string rfc) =>
        rfc.Trim().ToUpperInvariant().Replace(" ", "");

    private static bool EsEstructuralmenteValido(string normalizado, out string mensaje)
    {
        mensaje = string.Empty;

        if (_moral.IsMatch(normalizado) || _fisica.IsMatch(normalizado))
            return true;

        var longitud = normalizado.Length;

        mensaje = longitud switch
        {
            < 12 => $"RFC incompleto ({longitud} de 12–13 caracteres requeridos).",
            > 13 => $"RFC excede la longitud máxima ({longitud} caracteres; máximo 13).",
            12   => $"Formato inválido para persona moral. Se esperaba: AAA######XX.",
            13   => $"Formato inválido para persona física. Se esperaba: AAAA######XXX.",
            _    => "RFC con formato no reconocido."
        };

        return false;
    }

    // -------------------------------------------------------------------------
    // Presentación
    // -------------------------------------------------------------------------

    /// <summary>Enmascarado por defecto al convertir a cadena — principio de mínimo privilegio.</summary>
    public override string ToString() => ObtenerEnmascarado();
}

/// <summary>Tipo de persona jurídica según estructura del RFC.</summary>
public enum TipoPersonaRfc
{
    /// <summary>RFC de 12 caracteres: empresa, institución o sociedad.</summary>
    Moral,
    /// <summary>RFC de 13 caracteres: individuo con actividad empresarial o profesional.</summary>
    Fisica
}

/// <summary>
/// Estado de validación progresiva para retroalimentación visual en tiempo real.
/// Mapea directamente al color de borde del TextBox (§Módulo 08 README).
/// </summary>
public enum EstadoValidacionRfc
{
    /// <summary>Campo vacío — borde neutro.</summary>
    Vacio,
    /// <summary>Entrada parcial (&lt;12 chars) — borde neutro, sin error prematuro.</summary>
    Parcial,
    /// <summary>Formato completo y válido — borde verde.</summary>
    Valido,
    /// <summary>Longitud completa pero estructura inválida — borde rojo.</summary>
    Invalido
}
